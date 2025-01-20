using HR4you.Security.Contexts;
using HR4you.Security.Models;
using Microsoft.EntityFrameworkCore;

namespace HR4you.Security.Handler;

public class JwtBlacklistHandler
{
    private readonly UserContext _db;

    public JwtBlacklistHandler(UserContext db)
    {
        _db = db;
    }

    public bool IsTokenOk(string token)
    {
        PurgeOldTokens();
        var dbToken = _db.TokenBlacklist.FirstOrDefault(t => t.Token == token);
        return dbToken == null;
    }

    private void PurgeOldTokens()
    {
        var expireIn = DateTime.Now.AddMinutes(2);
        var tokensToRemove = _db.TokenBlacklist.Where(t => t.Expire > expireIn).ToList();
        _db.TokenBlacklist.RemoveRange(tokensToRemove);
        SaveChanges();
    }

    public void Add(string token)
    {
        PurgeOldTokens();
        var found = _db.TokenBlacklist.FirstOrDefault(t => t.Token == token) != null;
        if (!found)
        {
            _db.TokenBlacklist.Add(new BlacklistedToken(token));
        }

        SaveChanges();
    }

    private void SaveChanges()
    {
        var saved = false;
        while (!saved)
        {
            try
            {
                _db.SaveChanges();
                saved = true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                foreach (var entry in ex.Entries)
                {
                    if (entry.Entity is BlacklistedToken)
                    {
                        var proposedValues = entry.CurrentValues;
                        var databaseValues = entry.GetDatabaseValues();
                        if (databaseValues == null)
                        {
                        }

                        entry.OriginalValues.SetValues(proposedValues);
                    }
                    else
                    {
                        throw new NotSupportedException(
                            "Congrats if you see this because I don't know how to handle concurrency conflicts for "
                            + entry.Metadata.Name);
                    }
                }
            }
        }
    }
}
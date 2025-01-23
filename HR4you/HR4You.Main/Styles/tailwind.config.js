/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        "./**/*.{razor,html,cshtml}",
        "./wwwroot/index.html"
    ],
    theme: {
        extend: {},
    },
    plugins: [],
    safelist: [
        {
            pattern: /bg-(red|green|blue|gray|black)-(100|200|300|400|500|600|700|800)/,
        },
        "bg-opacity-50",
        "bg-black"
    ]
}


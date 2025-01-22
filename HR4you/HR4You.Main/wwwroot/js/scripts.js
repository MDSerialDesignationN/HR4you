document.addEventListener("DOMContentLoaded", () => {
    const dropdownButtons = document.querySelectorAll(".hr4you_dropdown-button");

    dropdownButtons.forEach(button => {
        button.addEventListener("click", () => {
            const dropdown = button.closest(".hr4you_dropdown");
            dropdown.classList.toggle("active");


        });
    });
});

window.getElementValue = (element) => {
    
    if (element.type === "checkbox") {
        console.log(element.checked.toString())
        return element.checked.toString();
    }
    return element.value;
};
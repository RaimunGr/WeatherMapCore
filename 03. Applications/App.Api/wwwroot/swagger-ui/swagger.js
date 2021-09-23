(function () {
    window.addEventListener("load", function () {
        setTimeout(function () {
            const logo = document.getElementsByClassName('link'); //For Changing The Link On The Logo Image
            logo[0].innerHTML = logo[0].innerHTML + "<i style='padding: 0 10px'>Raimun</i>";
            logo[0].href = "https://raimun.com/";
            logo[0].target = "_blank";
            logo[0].children[0].alt = "Raimun";
            logo[0].children[0].src = "/swagger-ui/favicon.png"; //For Changing The Logo Image
            logo[0].children[0].height = "80"
        });
    });
})();
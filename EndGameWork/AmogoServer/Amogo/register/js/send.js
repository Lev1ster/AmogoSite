function pushLogPass(lastName, login, name, password, telephone, urlAvatar) {
    $.ajax({
        type: "POST",
        url: "http://localhost:65388/Service/AccountService.svc/Accounts",
        contentType:"application\json",
        dataType: "json",
        data: JSON.stringify({ "isAdmin": 0, "lastName": lastName, "login": login, "name": name, "password": password, "telephone": telephone, "urlAvatar": urlAvatar })
    })
}

$(document).ready(function () {
    $('button').click(function (e) {
        // Stop form from sending request to server
        e.preventDefault(); {
        /*    lastName = document.getElementById("lastName").value;
            name = document.getElementById("name").value;
            log = document.getElementById("login").value;
            pass = document.getElementById("password").value;
            telephone = document.getElementById("telephone").value;
            urlAvatar = document.getElementById("urlAvatar").value;

            pushLogPass(lastName, log, name, pass, telephone, urlAvatar);*/

            document.location.href = document.location.href.replace(document.location.search, "")
            document.location.href = location.href.replace("register", "login")
        }
    })
})
function getTable(url) {
    $.getJSON(url, function (data) {
        var head_Item = "";
        var body_Item = "";

        var list = url.split("/");

        document.getElementById("MainName").innerHTML = "<a>" + list[list.length - 1] + "";

        head_Item += "<tr>";
        for (item in data[0]) {
            head_Item += "<th>" + item.toUpperCase() + "</th>";
        };
        head_Item += "</tr>";

        document.getElementById("headhead").innerHTML = head_Item;

        for (key in data) {
            body_Item += "<tr>";
            for (item in data[key]) {
                body_Item += "<td>" + data[key][item] + "</td>";
            };
            body_Item += "<tr>";
        };
        document.getElementById("bodybody").innerHTML = body_Item;
    });
};
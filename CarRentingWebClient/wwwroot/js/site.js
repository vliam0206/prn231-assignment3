$(() => {
    SetLocalStorageToken();
});

function SetLocalStorageToken() {
    var token = $("#token").val();
    localStorage.setItem("token", token);
}

function RemoveToken() {
    localStorage.removeItem("token");
}
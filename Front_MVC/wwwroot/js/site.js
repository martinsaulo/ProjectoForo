// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener("DOMContentLoaded", function () {
    loadOrderValues();
});

function ReplyComment(commentId) {
    const commentIdInput = document.getElementById("commentIdReply")

    commentIdInput.value = commentId
}

function EditComment(commentId) {
    const commentIdInput = document.getElementById("commentIdEdit")

    commentIdInput.value = commentId
}

function confirmPassword() {
    const password = document.getElementById('password')
    const confirm = document.getElementById('confirm-password')

    if (confirm.value != password.value) {

        confirm.setCustomValidity('Las contraseñas no coinciden.')

    } else if (!checkPassword(password.value)) {

        confirm.setCustomValidity('NO.')

    } else {

        confirm.setCustomValidity('')
    }

}

function checkPassword(password) {
    
    const regex = /^(?=.*\d)[A-Za-z\d]{8,32}$/; // 8 a 32 caracteres y al menos un número

    return regex.test(password)
}

function loadOrderValues() {

    const orderBy = document.getElementById('orderBy')
    const pageSize = document.getElementById('pageSize')
    const queryString = window.location.search;

    const urlParams = new URLSearchParams(queryString);


    if (urlParams.get('orderBy')) {
        orderBy.value = urlParams.get('orderBy')
    }

    if (urlParams.get('pageSize')) {
        pageSize.value = urlParams.get('pageSize')
    }
}
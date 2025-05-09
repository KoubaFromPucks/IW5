function showById(id) {
    document.getElementById(id).classList.remove('d-none');
}

function hideById(id) {
    document.getElementById(id).classList.add('d-none');
}

function setTextById(id, text) {
    document.getElementById(id).innerHTML = text;
}

export { showById, hideById, setTextById };

function yesnoCheck() {
    if (document.getElementById('yesCheck').checked) {
        document.getElementById('adminDiv').style.display = 'block';
        document.getElementById('userDiv').style.display = 'none'; 
    } else {
        document.getElementById('AdminCode').value = '';
        document.getElementById('adminDiv').style.display = 'none';
        document.getElementById('userDiv').style.display = 'block';
    }
}

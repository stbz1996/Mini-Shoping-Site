

function mostrarCrearCuenta() {
    document.getElementById('pagCrearCuenta').style.display = 'block';
    document.getElementById('contenido').style.display = 'none';
    document.getElementById('contactenos').style.display = 'none';
    document.getElementById('pagInicio').style.display = 'block';
}

function mostrarContactenos() {
    document.getElementById('contactenos').style.display = 'block';
    document.getElementById('contenido').style.display = 'none';
    document.getElementById('pagCrearCuenta').style.display = 'none';
    document.getElementById('pagInicio').style.display = 'block';
}

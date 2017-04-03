

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

function mostrarContenido() {
    //document.getElementById('pagInicio').style.display = 'none';
    //document.getElementById('contactenos').style.display = 'none';
    //document.getElementById('pagCrearCuenta').style.display = 'none';
}

function ejemplo(elemento) {
    alert('Gracias por pinchar ' + elemento.id + ' ' + elemento.title);

}
namespace admin;
public partial class Form1 : Form
{
    conexion conectar = new conexion();
    ciudadano elector = new ciudadano();
    public Form1()
    {
        InitializeComponent();
        CargarDatos();
    }
    //
    //FUNCION PARA OBTENER EL TIPO DE ACCESO QUE TIENE EL ADMINISTRADOR
    //
    private void CargarDatos()
    {
        try
        {
            int valor = conectar.ObtenerAcceso();
            if (valor == 1)
            {
                btnMostrarElector.Text = "Buscar";
                buscarElector.Text = "Buscar Ciudadano";
            }
            else if (valor == 2)
            {
                btnMostrarElector.Text = "Agregar";
                buscarElector.Text = "Agregar Ciudadano";
            }
            else { throw new Exception("valor fuera de rango"); }
        }
        catch (Exception ex) { throw new Exception("ERROR: " + ex); }
    }
    //
    //FUNCION PARA BUSCAR Y MOSTRAR USUARIOS O INGRESAR USUARIO A BD
    //
    private void btnMostrarUsuario_Click(object sender, EventArgs e)
    {
        //validacion de nulos
        if (RevisarNulo("ciudadano") != true)
        {
            try
            {
                //Crear variables para buscar o ingresar el nombre
                elector.getNombre = nombreElectorInput.Text;
                elector.getApellidoP = paternoElectorInput.Text;
                elector.getApellidoM = maternoElectorInput.Text;
                elector.getClaveUnica = claveRegistroInput.Text;
                if (btnMostrarElector.Text == "Agregar")
                {
                    //Agregar ciudadano
                    try
                    {
                        conectar.InsertarCiudadano(elector.getClaveUnica, CambiarTexto(elector.getNombre), CambiarTexto(elector.getApellidoP), CambiarTexto(elector.getApellidoM));
                        MessageBox.Show("Se agregaron los datos");

                        nombreElectorInput.Clear();
                        paternoElectorInput.Clear();
                        maternoElectorInput.Clear();
                        claveRegistroInput.Clear();
                        nombreElectorInput.Focus();

                    }
                    catch { throw new Exception("No se pudieron agregar los datos"); }
                }
                else if (btnMostrarElector.Text == "Buscar")
                {
                    //Buscar ciudadano
                    conectar.BuscarCiudadano(elector.getClaveUnica, elector.getNombre, elector.getApellidoP, elector.getApellidoM);

                    nombreElectorInput.Clear();
                    paternoElectorInput.Clear();
                    maternoElectorInput.Clear();
                    claveRegistroInput.Clear();
                    nombreElectorInput.Focus();
                }
                else { throw new Exception("CLAVE DEL BOTON NO ECONTRADA"); }

            }
            catch { throw new Exception("ALGO SALIO MAL AL BUSCAR O INGRESAR EL USUARIO"); }
        }
        else { MessageBox.Show("Uno o más campos se encuentran vacios", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
    }
    //
    //FUNCION PARA TERMNAR PROCESO
    //
    private void btnTerminar_Click(object sender, EventArgs e)
    {
        //validacion de nulos
        if (RevisarNulo("participantes") != true)
        {
            //obtener contraseñas
            var contrasenaAdministrador = conectar.ObtenerContraseña("administrador");
            var contrasenaPresidente = conectar.ObtenerContraseña("presidente");
            var contrasenaSecretario = conectar.ObtenerContraseña("secretario");
            var contrasenaEscrutador = conectar.ObtenerContraseña("escrutador");
            //comparar contraseñas
            if (contrasenaAdministrador == administradorInput.Text && contrasenaPresidente == presidenteInput.Text && contrasenaSecretario == secretarioInput.Text && contrasenaEscrutador == escrutadorInput.Text)
            {
                var msg = MessageBox.Show("¿Desea terminar y proceder al conteo?", "Terminar", MessageBoxButtons.OKCancel);
                if (msg == DialogResult.OK)
                {
                    //modificar un acceso en la BD para que la ventana de resultados inicie conteo
                    this.Close();
                }
            }
            else { MessageBox.Show("Las contraseñas no coinciden. verifique nuevamente", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
        }
        else { MessageBox.Show("Uno o más campos se encuentran vacios", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
    }
    //
    //FUNCION PARA CAMBIAR TEXTO INGRESADO (Primera Letra Mayuscula)
    //
    private string CambiarTexto(string sender)
    {
        try
        {
            string texto = sender.ToLower() ?? throw new Exception();
            return texto.First().ToString().ToUpper() + texto.Substring(1);
        }
        catch { return string.Empty; }
    }
    //
    //FUNCION PARA REVISAR NULOS POR BLOQUE (CIUDADANO, PARTICIPANTES)
    //
    public bool RevisarNulo(string bloque)
    {
        bool retorno = true;
        switch (bloque)
        {
            case "ciudadano":
                retorno = string.IsNullOrEmpty(nombreElectorInput.Text)
                || string.IsNullOrEmpty(paternoElectorInput.Text)
                || string.IsNullOrEmpty(maternoElectorInput.Text)
                || string.IsNullOrEmpty(claveRegistroInput.Text);
                break;

            case "participantes":
                retorno = string.IsNullOrEmpty(administradorInput.Text)
                || string.IsNullOrEmpty(presidenteInput.Text)
                || string.IsNullOrEmpty(secretarioInput.Text)
                || string.IsNullOrEmpty(escrutadorInput.Text);
                break;

            default: throw new Exception("Bloque no reconocido");
        }
        return retorno;
    }
}
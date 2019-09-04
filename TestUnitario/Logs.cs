using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestUnitario
{
    [TestClass]
    public class Logs
    {
        [TestMethod]
        public void ResultadoEsperado()
        {

            // Definición de parámetros para la prueba
            bool consola = true;
            bool archivo = false;
            bool bDatos = false;
            bool esperado = false;
            

            // Llamado de método de clase
            bool rdo = Logeo.JobbLogger.ControlarInicio(consola, archivo, bDatos);

            // Controlar que la prueba cumpla con el valor esperado
            Assert.AreEqual(esperado, rdo);

        }

        [TestMethod]
        public void ResultadoNoEsperado()
        {

            Assert.ThrowsException<System.Exception>(() => Logeo.JobbLogger.ControlarInicio(false, false, false));


        }
    }
}

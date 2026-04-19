using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKU.Scripts.ExceptionSystem
{
    /// <summary>
    /// Representa a classe base abstrata para exceções específicas da aplicação MKU.Scripts.
    /// </summary>
    public abstract class ApplicationException : Exception
    {
        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="ApplicationException"/> com uma mensagem de erro especificada.
        /// </summary>
        /// <param name="message">A mensagem que descreve o erro.</param>
        public ApplicationException(string? message) : base(message){}
    }
}


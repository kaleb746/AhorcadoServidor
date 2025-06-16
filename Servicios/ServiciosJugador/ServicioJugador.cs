using Servicios.Dtos;
using System;
using System.Collections.Generic;

namespace Servicios.ServiciosJugador
{
    public class ServicioJugador : IJugadorManager
    {
        public int RegistrarJugador(JugadorDTO jugador)
        {
            var dao = new JugadorDAO();
            return dao.RegistrarJugador(jugador);
        }
        public int ActualizarJugador(JugadorDTO jugador)
        {
            var dao = new JugadorDAO();
            return dao.ActualizarJugador(jugador);
        }
        public int EliminarJugador(int idJugador)
        {
            var dao = new JugadorDAO();
            return dao.EliminarJugador(idJugador);
        }
        public List<JugadorDTO> ObtenerTodosLosJugadores()
        {
            var dao = new JugadorDAO();
            return dao.ObtenerTodos();
        }
    }
}

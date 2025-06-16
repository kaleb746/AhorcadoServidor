using System;
using System.Collections.Concurrent;
using Servicios.ServiciosPartida;

namespace Servicios.Utilidades
{
    public static class CallbackManager
    {
        private static readonly ConcurrentDictionary<int, IPartidaCallback> CallbacksPorJugador = new ConcurrentDictionary<int, IPartidaCallback>();

        public static void RegistrarCallback(int idJugador, IPartidaCallback callback)
        {
            CallbacksPorJugador[idJugador] = callback;
        }

        public static bool TryObtenerCallback(int idJugador, out IPartidaCallback callback)
        {
            return CallbacksPorJugador.TryGetValue(idJugador, out callback);
        }

        public static void EliminarCallback(int idJugador)
        {
            CallbacksPorJugador.TryRemove(idJugador, out _);
        }
        public static bool IntentarNotificar(int idJugador, string usernameInvitado)
        {
            if (TryObtenerCallback(idJugador, out var callback))
            {
                try
                {
                    callback.NotificarJugadorUnido(usernameInvitado);
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Callback Error] {ex.Message}");
                    EliminarCallback(idJugador);
                }
            }
            return false;
        }
        public static bool IntentarNotificarIntentoLetra(int idJugador, char letra, bool acierto, string estadoActual)
        {
            if (TryObtenerCallback(idJugador, out var callback))
            {
                try
                {
                    callback.NotificarIntentoLetra(letra, acierto, estadoActual);
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Callback Error - IntentarNotificarIntentoLetra] {ex.Message}");
                    EliminarCallback(idJugador);
                }
            }
            return false;
        }
        public static void IntentarNotificarFinDePartida(int idJugador, string mensaje, bool ganaste)
        {
            if (CallbacksPorJugador.TryGetValue(idJugador, out var callback))
            {
                try
                {
                    callback.NotificarFinDePartida(mensaje, ganaste);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Callback] Error al notificar fin de partida a jugador {idJugador}: {ex.Message}");
                }
            }
        }
        public static void NotificarFinDePartida(int idGanador, int idPerdedor)
        {
            IntentarNotificarFinDePartida(idGanador, "¡Has ganado la partida!", true);
            IntentarNotificarFinDePartida(idPerdedor, "Has perdido la partida. ¡Sigue intentando!", false);
        }

    }
}
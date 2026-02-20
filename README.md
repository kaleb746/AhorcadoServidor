# 🎮 Ahorcado - Servidor

## 📌 Descripción

**Ahorcado Servidor** es la parte backend del juego clásico del Ahorcado. Está desarrollado en **C# utilizando .NET Framework** y expone sus funcionalidades mediante **WCF (Windows Communication Foundation)** para permitir la comunicación con aplicaciones cliente bajo una arquitectura cliente-servidor.

El sistema implementa la lógica completa del juego, administración de jugadores y gestión de partidas, manteniendo una estructura organizada en capas para garantizar mantenibilidad y escalabilidad.

---

## ✨ Características

### 🏗️ Arquitectura en capas

- **Capa de Host**
  - Inicializa y expone el servicio WCF.
  - Configura el `ServiceHost` para recibir solicitudes de clientes.

- **Capa de Servicios (WCF)**
  - Expone las operaciones del juego.
  - Actúa como fachada entre los clientes y la lógica de negocio.

- **Capa de Lógica de Negocio**
  - Implementa reglas del juego (validación de letras, intentos, estado de partida).
  - Gestiona jugadores y sesiones.
  - Controla la creación y administración de partidas.

- **Capa de Acceso a Datos**
  - Maneja la persistencia de información.
  - Utiliza patrones como DAO y DTO para separar responsabilidades.

---

## 🎯 Funcionalidades Principales

- 🔐 Registro de jugadores  
- 🔑 Inicio de sesión  
- 🎮 Creación de partidas  
- 👥 Unión a partidas existentes  
- 🔤 Selección de palabras por categoría y dificultad  
- ✏️ Intento de letras  
- 📊 Consulta del estado actual de la partida  
- 📜 Historial de partidas por jugador  

---

## 🛠️ Tecnologías Utilizadas

- C#
- .NET Framework
- WCF (Windows Communication Foundation)
- Patrón DAO
- DTO (Data Transfer Objects)
- Arquitectura Cliente-Servidor

---
## 📁 Estructura del Proyecto

AhorcadoServidor  
├── Host  
│   └── Program.cs  
│  
├── Servicios  
│   ├── ServicioPrincipal  
│   ├── ServiciosJugador  
│   ├── ServiciosPalabra  
│   ├── ServiciosPartida  
│   └── ServiciosSesion  
│  
└── Modelo  
    ├── DTOs  
    ├── Entidades  
    └── AccesoDatos  

---

## ▶️ Ejecución del Proyecto

1. Clonar el repositorio:

```bash
git clone https://github.com/kaleb746/AhorcadoServidor.git

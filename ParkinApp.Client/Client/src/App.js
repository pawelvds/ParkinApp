import { useState, useEffect } from "react";
import { Routes, Route } from "react-router-dom";
import AuthService from "./services/AuthService";
import Login from "./components/user/Login";
import Register from "./components/user/Register";
import Home from "./components/home/Home";
import Logout from "./components/user/Logout";
import Navbar from "./components/layout/Navbar";

function App() {
  const [currentUser, setCurrentUser] = useState(undefined);
  const [socket, setSocket] = useState(null);

  useEffect(() => {
    const user = AuthService.getCurrentUser();

    if (user) {
      setCurrentUser(user);
    }

    // Connect to WebSocket server
    const newSocket = new WebSocket("ws://localhost:5000");
    setSocket(newSocket);

    // Disconnect from WebSocket server on unmount
    return () => newSocket.close();
  }, []);
  
  useEffect(() => {
    const user = AuthService.getCurrentUser();
    if (user && AuthService.isTokenExpired(user.accessToken)) {
      AuthService.refreshAccessToken(user.refreshToken).catch((error) => {
        console.error("Error refreshing access token:", error);
        AuthService.logout().then(() => {
          window.location.href = "/login";
        });
      });
    }
  }, []);

  const handleLogout = () => {
    AuthService.logout().then(() => setCurrentUser(undefined));
  };

  return (
      <div>
        <Navbar currentUser={currentUser} onLogout={handleLogout} />

        <div className="container mt-3">
          <Routes>
            <Route
                path="/home"
                element={<Home currentUser={currentUser} token={AuthService.getAccessToken()} socket={socket} />}
            />
            <Route path="/login" element={<Login />} />
            <Route path="/register" element={<Register />} />
            <Route path="/logout" element={<Logout onLogout={handleLogout} />} />
          </Routes>
        </div>
      </div>
  );
}

export default App;

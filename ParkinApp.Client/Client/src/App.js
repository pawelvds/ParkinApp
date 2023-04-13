import { useState, useEffect } from "react";
import { Routes, Route, Link } from "react-router-dom";
import AuthService from "./services/AuthService";
import Login from "./components/user/Login";
import Register from "./components/user/Register";
import Home from "./components/home/Home";
import Logout from "./components/user/Logout";

function App() {
  const [currentUser, setCurrentUser] = useState(undefined);

  useEffect(() => {
    const user = AuthService.getCurrentUser();

    if (user) {
      setCurrentUser(user);
    }
  }, []);

  const handleLogout = () => {
    AuthService.logout().then(() => setCurrentUser(undefined));
  };

  return (
      <div>
        <nav className="navbar navbar-expand-lg navbar-dark bg-dark">
          <div className="container-fluid">
            <Link to={"/home"} className="navbar-brand">
              ParkinApp
            </Link>
            <button
                className="navbar-toggler"
                type="button"
                data-bs-toggle="collapse"
                data-bs-target="#navbarNav"
                aria-controls="navbarNav"
                aria-expanded="false"
                aria-label="Toggle navigation"
            >
              <span className="navbar-toggler-icon"></span>
            </button>
            <div className="collapse navbar-collapse" id="navbarNav">
              {currentUser ? (
                  <div className="navbar-nav ms-auto">
                    <li className="nav-item">
                      <button className="nav-link btn btn-link" onClick={handleLogout}>
                        Logout
                      </button>
                    </li>
                  </div>
              ) : (
                  <div className="navbar-nav ms-auto">
                    <li className="nav-item">
                      <Link to={"/login"} className="nav-link">
                        Login
                      </Link>
                    </li>

                    <li className="nav-item">
                      <Link to={"/register"} className="nav-link">
                        Register
                      </Link>
                    </li>
                  </div>
              )}
            </div>
          </div>
        </nav>

        <div className="container mt-3">
          <Routes>
            <Route path="/home" element={<Home currentUser={currentUser} />} />
            <Route path="/login" element={<Login />} />
            <Route path="/register" element={<Register />} />
            <Route path="/logout" element={<Logout onLogout={handleLogout} />} />
          </Routes>
        </div>
      </div>
  );
}

export default App;

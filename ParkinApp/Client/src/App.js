import { useState, useEffect } from "react";
import { Routes, Route, Link } from "react-router-dom";
import AuthService from "./services/AuthService";
import Login from "./components/user/Login";
import Register from "./components/user/Register";
import Home from "./components/home/Home";
import "bootstrap/dist/css/bootstrap.min.css";

function App() {
  const [currentUser, setCurrentUser] = useState(undefined);

  useEffect(() => {
    const user = AuthService.getCurrentUser();

    if (user) {
      setCurrentUser(user);
    }
  }, []);

  const logOut = () => {
    AuthService.logout();
  };

  return (
      <div>
        <nav className="navbar navbar-expand navbar-dark bg-dark">
          <div className="navbar-nav mr-auto">
            <li className="nav-item">
              <Link to={"/home"} className="nav-link">
                Home
              </Link>
            </li>
          </div>

          {currentUser ? (
              <div className="navbar-nav ms-auto">
                <li className="nav-item">
                  <a href="/login" className="nav-link" onClick={logOut}>
                    Logout
                  </a>
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
        </nav>

        <div className="container mt-3">
          <Routes>
            <Route path="/home" element={<Home />} />
            <Route path="/login" element={<Login />} />
            <Route path="/register" element={<Register />} />
          </Routes>
        </div>
      </div>
  );
}

export default App;
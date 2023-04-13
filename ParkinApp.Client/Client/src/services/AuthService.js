import axios from "axios";
import jwt_decode from "jwt-decode";
import API_ENDPOINT from "../config";

const isTokenExpired = (token) => {
    try {
        const decodedToken = jwt_decode(token);
        return decodedToken.exp < Date.now() / 1000;
    } catch (error) {
        return true;
    }
};

const isRefreshTokenExpired = (token) => {
    try {
        const decodedToken = jwt_decode(token);
        return decodedToken.exp < Date.now() / 1000;
    } catch (error) {
        return true;
    }
};

const refreshAccessToken = async (refreshToken) => {
    try {
        const response = await axios.post(API_ENDPOINT + "/User/refresh-token", { refreshToken });
        if (response.data.accessToken) {
            const user = getCurrentUser();
            user.accessToken = response.data.accessToken;
            localStorage.setItem("user", JSON.stringify(user));
            return response.data;
        }
    } catch (error) {
        console.error("Error refreshing access token: ", error);
        logout();
        // Redirect to the login page instead of reloading the current page
        window.location.href = "/login";
    }
};

const scheduleRefresh = (user) => {
    if (!user || !user.accessToken || !user.refreshToken) {
        return;
    }

    const decodedToken = jwt_decode(user.accessToken);
    const expiresIn = decodedToken.exp * 1000 - Date.now() - 300000; // 5 mins earlier

    if (expiresIn > 0) {
        setTimeout(async () => {
            try {
                const { refreshToken } = user;
                const refreshedUser = await refreshAccessToken(refreshToken);
                scheduleRefresh(refreshedUser);
            } catch (error) {
                console.error("Error refreshing access token: ", error);
            }
        }, expiresIn);
    }
};

axios.interceptors.request.use(
    async (config) => {
        const user = getCurrentUser();
        if (user && user.accessToken && isTokenExpired(user.accessToken)) {
            if (isRefreshTokenExpired(user.refreshToken)) {
                await logout();
                window.location.reload();
            } else {
                try {
                    const { refreshToken } = user;
                    const refreshedUser = await refreshAccessToken(refreshToken);
                    config.headers.Authorization = `Bearer ${refreshedUser.accessToken}`;
                } catch (error) {
                    console.error("Error refreshing access token: ", error);
                }
            }
        } else if (user && user.accessToken) {
            config.headers.Authorization = `Bearer ${user.accessToken}`;
        }
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);

const register = (username, password) => {
    return axios
        .post(API_ENDPOINT + "/User/register", {
            username,
            password,
        })
        .then((response) => {
            if (response.data.accessToken) {
                localStorage.setItem("user", JSON.stringify(response.data));
            }

            return response.data;
        });
};

const login = (username, password) => {
    return axios
        .post(API_ENDPOINT + "/User/login", {
            username,
            password,
        })
        .then((response) => {
            if (response.data.accessToken) {
                localStorage.setItem("user", JSON.stringify(response.data));
                scheduleRefresh(response.data);
            }

            return response.data;
        });
};

const logout = () => {
    const user = getCurrentUser();
    if (user && user.refreshToken) {
        return axios
            .post(API_ENDPOINT + "/User/logout", { refreshToken: user.refreshToken })
            .then(() => {
                localStorage.removeItem("user");
            })
            .catch((error) => {
                console.error(error);
            });
    } else {
        localStorage.removeItem("user");
        return Promise.resolve();
    }
};

const getCurrentUser = () => {
    return JSON.parse(localStorage.getItem("user"));
};

const AuthService = {
    register,
    login,
    logout,
    getCurrentUser,
};

export default AuthService;

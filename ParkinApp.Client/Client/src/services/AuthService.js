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

const refreshAccessToken = (refreshToken) => {
    return axios
        .post(API_ENDPOINT + "/User/refresh-token", { refreshToken })
        .then((response) => {
            if (response.data.accessToken) {
                const user = getCurrentUser();
                user.accessToken = response.data.accessToken;
                localStorage.setItem("user", JSON.stringify(user));
            }

            return response.data;
        });
};

axios.interceptors.request.use(
    async (config) => {
        const user = getCurrentUser();
        if (user && user.accessToken && isTokenExpired(user.accessToken)) {
            try {
                const { refreshToken } = user;
                const refreshedUser = await refreshAccessToken(refreshToken);
                config.headers.Authorization = `Bearer ${refreshedUser.accessToken}`;
            } catch (error) {
                console.error("Error refreshing access token: ", error);
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

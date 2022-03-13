import axios from "axios";

//axios.interceptors.request.use((request) => {console.log(request)});

export const addUser = async (username: string, password: string) => {
    var response = await axios.post("http://localhost:5000/auth/register", {
        username,
        password
    });
    console.log(response.data);
};

const makeRequest = () => {

};
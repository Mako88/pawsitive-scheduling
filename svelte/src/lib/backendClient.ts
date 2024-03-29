import axios from "axios";

//axios.interceptors.request.use((request) => {console.log(request)});

export const addUser = async (username: string, password: string) => {
    await makeRequest("auth/register", {
        username,
        password
    });
};

const makeRequest = async (path: string, body: object) => {
  var response = await axios.post(`https://localhost:5001/${path}`, body);
  console.log(response.data);
};
import axios from "axios";

export default class PlopService {
    static getData(): Promise<string> {
        return axios.get<string>("https://localhost:7001/plop")
            .then((response) => response.data);
    }
}
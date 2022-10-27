import axios from "axios";

export default class PlopService {
    static getData(accessToken: string | null): Promise<string> {
        return axios.get<string>("https://localhost:7001/plop",
            {
                headers: {Authorization: `Bearer ${accessToken}`}
            })
            .then((response) => response.data);
    }
}
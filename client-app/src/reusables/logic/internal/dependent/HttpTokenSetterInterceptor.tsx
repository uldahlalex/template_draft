import {http} from "./http.ts";
import {useAtom} from "jotai";
import {jwtAtom} from "../../../state/external.ts";

export const HttpTokenSetterInterceptor = () => {

    const [jwt] = useAtom(jwtAtom);

    http.instance.interceptors.request.use(config => {
        config.headers.Authorization = jwt
        return config;
    });
    return null;
}


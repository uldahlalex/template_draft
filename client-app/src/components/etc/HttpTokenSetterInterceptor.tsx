import {useAtom} from "jotai";
import {jwtAtom} from "../../reusables/state/states.ts";
import {http} from "../../reusables/logic/logic.ts";

export const HttpTokenSetterInterceptor = () => {

    const [jwt] = useAtom(jwtAtom);

    http.instance.interceptors.request.use(config => {
        config.headers.Authorization = jwt
        return config;
    });
    return null;
}


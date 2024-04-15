import {http} from "../../../reusables/logic/external.ts";
import {useAtom} from "jotai";
import {jwtAtom} from "../../../reusables/state/external.ts";

export default function HttpTokenSetterInterceptor() {

    const [jwt] = useAtom(jwtAtom);

    http.instance.interceptors.request.use(config => {
        config.headers.Authorization = jwt
        return config;
    });
    return null;
}


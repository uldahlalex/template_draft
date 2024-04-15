import {AxiosError, AxiosResponse} from "axios";
import toast from "react-hot-toast";
import {http} from "../../reusables/logic/logic.ts";
import {useAtom} from "jotai";
import {jwtAtom} from "../../reusables/state/states.ts";

interface ProblemDetails {
    type: string;
    title: string;
    status: number;
    detail?: string;
    instance?: string;
    [key: string]: any;
}
export const HttpErrorInterceptor = ()  => {

    const [, setJwt] = useAtom(jwtAtom);

    http.instance.interceptors.response.use((response: AxiosResponse) => {
        if (response.status === 401) handleUnauthorizedAccess(); //todo is this in usage or is only onRejected in usage?
        return response;
    }, (error: AxiosError<any, any>) => {
        if (error.response && error.response.status === 401) handleUnauthorizedAccess();
        const problemDetails = error.response?.data as ProblemDetails;
        if (problemDetails) toast.error(problemDetails.detail!, { id: `error-${problemDetails.detail}` });
        return Promise.reject(error);
    });
    const handleUnauthorizedAccess = () => setJwt('')
    return null;
}


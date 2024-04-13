import {useAtom} from "jotai/index";
import {AxiosError, AxiosResponse} from "axios";
import toast from "react-hot-toast";
import {userAtom, UserAtom} from "../../reusables/state/external.ts";
import {http} from "../../reusables/logic/internal/http.ts";

interface ProblemDetails {
    type: string;
    title: string;
    status: number;
    detail?: string;
    instance?: string;
    [key: string]: any;
}
export default function SetupHttpClient() {

    const [, setUser] = useAtom<UserAtom | null>(userAtom);


    http.instance.interceptors.request.use(config => {

        config.headers.Authorization = localStorage.getItem('token') || '';

        return config;
    });
    http.instance.interceptors.response.use((response: AxiosResponse) => {
        if (response.status === 401) {
            handleUnauthorizedAccess();
        }
        return response;
    }, (error: AxiosError<any, any>) => {
        if (error.response && error.response.status === 401) {
            handleUnauthorizedAccess();
        }
        const problemDetails = error.response?.data as ProblemDetails;
        if (problemDetails) {
            toast.error(problemDetails.detail!, { id: `error-${problemDetails.detail}` });
        }
            return Promise.reject(error);
    });
    const handleUnauthorizedAccess = () => {
        localStorage.removeItem('token');
        setUser(null)
        toast.error('Unauthorized access');
    }
    return null;
}


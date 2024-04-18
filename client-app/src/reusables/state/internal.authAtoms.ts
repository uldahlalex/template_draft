import {atomWithStorage} from "jotai/utils";
import {atom} from "jotai/index";
import toast from "react-hot-toast";

export const jwtAtom = atomWithStorage<string>('token', '');

export interface User {
    username: string;
    id: number;
}

export const userAtom = atom<User | null>((get) => {
    const token = get(jwtAtom);
    if (token == '') return null;
    const payload = decodeJwt(token)
    if (payload.Id === undefined || payload.Username === undefined) toast('Invalid token!');
    const user: User = {
        username: payload.Username,
        id: payload.Id
    }
    return user;
});

export function decodeJwt(jwt) {
    function base64UrlToBase64(input) {
        let base64String = input.replace(/-/g, '+').replace(/_/g, '/');
        while (base64String.length % 4) {
            base64String += '=';
        }
        return base64String;
    }

    function decodeBase64Json(base64) {
        const decodedString = atob(base64);
        return JSON.parse(decodedString);
    }

    const parts = jwt.split('.');

    if (parts.length !== 3) {
        throw new Error('Invalid JWT: The token must have three parts.');
    }
    const payload = decodeBase64Json(base64UrlToBase64(parts[1]));
    //user payloads such as Id and Username can be accessed paylod.Id and payload.Username
    return payload;
}


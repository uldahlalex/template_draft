import {atom} from 'jotai';
import {decodeJwt} from "../../logic/external.ts";
import {jwtAtom} from "./jwtAtom.ts";
import toast from "react-hot-toast";

export interface User {
    username: string;
    id: number;
}
export const userAtom = atom<User | null>((get) => {
    const token = get(jwtAtom);
    if(token == '') return null;
    const payload = decodeJwt(token)
    if(payload.Id === undefined || payload.Username === undefined) toast('Invalid token!');
    const user: User = {
        username: payload.Username,
        id: payload.Id
    }
    return user;
});





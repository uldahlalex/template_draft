import {atomWithStorage} from "jotai/utils";
import {atom} from "jotai/vanilla/atom";
import {decodeJwt} from "../../logic/external.ts";
import {User} from "./user.ts";

export const jwtAtom = atomWithStorage<string>('token', '');



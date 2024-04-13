import {UserAtom} from "../atoms/internal/userAtom.ts";

export function decodeJwt(jwt): UserAtom {
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
    const u: UserAtom = {
        id: payload.Id,
        username: payload.Username,
    };
    return u;
}
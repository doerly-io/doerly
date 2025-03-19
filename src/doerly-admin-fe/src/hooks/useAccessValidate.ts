// import { useCallback, useContext } from 'react';
// import { AuthoritiesContext } from 'components/AuthoritiesProvider';
// import AuthModes from 'constants/authoritiesValidationModes';
//
// const functionsToModes = {
//   [AuthModes.ANY]: (
//     authorities: string[],
//     ownedAuthorities: string[]
//   ) => authorities
//     .some((auth: string) => ownedAuthorities.includes(auth)),
//   [AuthModes.ALL]: (
//     authorities: string[],
//     ownedAuthorities: string[]
//   ) => authorities
//     .every((auth: string) => ownedAuthorities.includes(auth)),
// };
//
// function useAccessValidate() {
//   const ownedAuthorities = useContext<string[]>(AuthoritiesContext);
//
//   return useCallback(
//     (
//       neededAuthorities: string[] | string,
//       mode: AuthModes = AuthModes.ANY
//     ) => {
//       const authorities = (Array.isArray(neededAuthorities)
//         ? neededAuthorities
//         : [neededAuthorities]);
//       return functionsToModes[mode](authorities, ownedAuthorities);
//     },
//     [ownedAuthorities]
//   );
// }
//
// export default useAccessValidate;

export const stub = 0;

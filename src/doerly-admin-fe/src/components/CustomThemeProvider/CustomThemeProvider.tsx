import React, {
  createContext,
  useCallback,
  useMemo,
  useState,
} from 'react';

import defaultTheme from './themes/default';

type ThemeNames = 'default' // | 'someCustomThemeName';

const ThemesToThemeNames = {
  default: defaultTheme,
};

export const ThemeContext = createContext<IContextValue>({
  changeTheme: () => {},
  theme: {},
});

const CustomThemeProvider = ({
  children,
  themeName: inputThemeName = 'default',
}: IProps) => {
  const [state, setState] = useState({
    themeName: inputThemeName,
  });

  const changeTheme = useCallback((themeName: ThemeNames) => {
    setState(prevState => ({
      ...prevState,
      themeName,
    }));
  }, []);

  const contextValue = useMemo(() => ({
    changeTheme,
    theme: ThemesToThemeNames[state.themeName],
  }), [state.themeName, changeTheme]);

  return (
    <ThemeContext.Provider value={contextValue}>
      {children}
    </ThemeContext.Provider>
  );
};

interface IProps {
  children: React.ReactNode,
  themeName?: ThemeNames,
}

interface IContextValue {
  changeTheme: (themeName: ThemeNames) => void,
  theme: any,
}

export default CustomThemeProvider;

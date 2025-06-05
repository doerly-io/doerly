const FONT_FAMILY = '"Segoe UI", sans-serif';

const theme = {
  button: {
    primary: {
      color: {
        background: '#298967',
        hover: '#34d399',
        text: '#18181b',
      },
    },
    secondary: {
      color: {
        background: '#298967',
        hover: '#34d399',
        text: '#111111',
      },
    },
  },
  card: {
    background: {
      edit: '#EBF1F6',
      error: '#F6CCCE',
      info: '#D9E5EE',
      paper: '#18181b',
      success: '#D0E4D6',
      warning: '#FAEDD5',
    },
  },
  circularProgress: {
    color: '#34d399',
  },
  colors: {
    black: '#111111',
    blueDark: '#D9E5EE',
    blueLight: '#EBF1F6',
    cobalt: '#34d399',
    greenDark: '#2DC100',
    greenLight: '#D0E4D6',
    greyDark: 'rgba(119,119,119,0.5)',
    greyLight: '#E6E6E6',
    purpleLight: '#F3E5F5',
    redDark: '#D3000C',
    redLight: '#F6CCCE',
    secondary: '#298967',
    white: '#FFFFFF',
    yellowDark: '#E9A631',
    yellowLight: '#FAEDD5',
  },
  header: {
    background: '#121212',
    height: 58,
  },
  hover: {
    background: 'rgba(0, 0, 0, 0.05)',
    backgroundLight: '#141a16',
    selected: {
      background: '#141a16',
    },

  },
  icon: {
    color: {
      button: '#FFFFFF',
      error: '#D3000C',
      header: '#34d399',
      page: '#34d399',
    },
  },
  iconButton: {
    border: {
      color: 'rgba(255, 255, 255, 0.5)',
    },
  },
  link: {
    color: '#298967',
  },
  loading: {
    background: '#18181b',
  },
  pageContainer: {
    border: '#121212',
    container: {
      background: '#121212',
    },
    content: {
      width: 1024,
    },
  },
  select: {
    color: {
      border: '#34d399',
      focus: '#34d399',
      label: '#34d399',
      text: '#FFFFFF',
    },
  },
  sideBar: {
    background: '#121212',
    border: '#34d399',
    width: 220,
  },
  spacing: (x: number = 1) => x * 8,
  tabs: {
    background: '#FFFFFF',
  },
  textField: {
    color: {
      error: '#D3000C',
      info: '#1C7FDB',
      primary: '#34d399',
      secondary: '#059669',
      success: '#007504',
      warning: '#E9A631',
    },
  },
  typography: {
    color: {
      error: '#D3000C',
      info: '#1C7FDB',
      paper: '#FFFFFF',
      primary: '#34d399',
      secondary: '#298967',
      success: '#007504',
      warning: '#E9A631',
    },
    variants: {
      capitalized: {
        fontFamily: FONT_FAMILY,
        fontSize: '14px',
        fontWeight: 400,
        letterSpacing: '0.03333em',
        lineHeight: 1.3,
        textTransform: 'capitalize',
      },
      caption: {
        fontFamily: FONT_FAMILY,
        fontSize: '12px',
        fontWeight: 400,
        letterSpacing: '0.03333em',
        lineHeight: 1.3,
      },
      default: {
        fontFamily: FONT_FAMILY,
        fontSize: '14px',
        fontWeight: 400,
        letterSpacing: '0.03333em',
        lineHeight: 1.3,
      },
      subtitle: {
        fontFamily: FONT_FAMILY,
        fontSize: '16px',
        fontWeight: 400,
        letterSpacing: '0.03333em',
        lineHeight: 1.3,
      },
      title: {
        fontFamily: FONT_FAMILY,
        fontSize: '20px',
        fontWeight: 400,
        letterSpacing: '0.03333em',
        lineHeight: 1.3,
      },
    },
  },
};

export default theme;

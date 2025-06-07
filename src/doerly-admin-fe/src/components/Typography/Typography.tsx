import React from 'react';
import TypographyMUI from '@mui/material/Typography';

import useTheme from 'hooks/useTheme';

type Alignment = 'center'
  | 'inherit'
  | 'justify'
  | 'left'
  | 'right';

type Colors = 'paper'
  | 'primary'
  | 'secondary'
  | 'error'
  | 'info'
  | 'success'
  | 'warning'
  | 'inherit';

type Variants = 'capitalized'
  | 'caption'
  | 'default'
  | 'subtitle'
  | 'title';

interface IProps {
  align?: Alignment;
  children: React.ReactNode,
  color?: Colors;
  noWrap?: boolean,
  variant?: Variants,
  wordBreak?: string,
}

const Typography = ({
  align = 'inherit',
  children,
  color = 'primary',
  noWrap = false,
  variant = 'default',
  wordBreak = 'normal',
}: IProps) => {
  const { theme } = useTheme();
  return (
    <TypographyMUI
      align={align}
      noWrap={noWrap}
      sx={{
        ...theme.typography.variants[variant],
        caretColor: '#FFFFFF',
        color: color === 'inherit'
          ? 'inherit'
          : theme.typography.color[color],
        wordBreak,
      }}
    >
      {children}
    </TypographyMUI>
  );
};

export default Typography;

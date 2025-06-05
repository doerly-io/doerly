import React from 'react';
import SvgIcon from '../SvgIcon';
import useTheme from 'hooks/useTheme';

const Plus = ({ color = 'page', size = 32 }: IProps) => {
  const { theme } = useTheme();
  const actualColor = theme.icon.color[color] || color;
  return (
    <SvgIcon
      style={{
        height: `${size}px`,
        width: `${size}px`,
      }}
      viewBox="0 0 24 24"
    >
      <path
        d="M 19 13 h -6 v 6 h -2 v -6 H 5 v -2 h 6 V 5 h 2 v 6 h 6 v 2 Z"
        fill={actualColor}
      />
    </SvgIcon>
  );
};

interface IProps {
  color?: 'header' | 'page' | string;
  size?: number;
}

export default Plus;

import React from 'react';
import SvgIcon from '../SvgIcon';
import useTheme from 'hooks/useTheme';

/* eslint-disable max-len */
const Catalog = ({
  color = 'page',
  size = 32,
}: IProps) => {
  const { theme } = useTheme();
  const actualColor = theme.icon.color[color] || color;
  return (
    <SvgIcon
      style={{
        height: `${size}px`,
        width: `${size}px`,
      }}
      viewBox="0 0 32 32"
    >
      <g>
        <path
          d="M24 25h-3v-3h3zm5-3h-3v3h3zm-5 5h-3v3h3zm5 0h-3v3h3zM20 8h-8v2h8zm-3 20H6v-4h2v-2H6v-5h2v-2H6v-5h2V8H6V4h18v15h2V4c0-1.1-.9-2-2-2H6c-1.1 0-2 .9-2 2v4H2v2h2v5H2v2h2v5H2v2h2v4c0 1.1.9 2 2 2h11zm3-13h-8v2h8z"
          fill={actualColor}
        />
      </g>
    </SvgIcon>
  );
};

interface IProps {
  color?: 'header' | 'page' | string,
  size?: number,
}

export default Catalog;

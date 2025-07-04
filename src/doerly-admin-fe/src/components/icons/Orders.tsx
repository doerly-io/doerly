import React from 'react';
import SvgIcon from '../SvgIcon';
import useTheme from 'hooks/useTheme';

/* eslint-disable max-len */
const Orders = ({
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
          d="M28 13h-2v-2c0-2.8-2.2-5-5-5h-3V4h3c3.9 0 7 3.1 7 7zm-6 7h3v2h-3zm0 4h3v2h-3z"
          fill={actualColor}
        />
        <path
          d="M27 30h-5v-2h5V18h-7v9c0 1.7-1.3 3-3 3s-3-1.3-3-3v-3h2v3c0 .6.4 1 1 1s1-.4 1-1V16h11v12c0 1.1-.9 2-2 2m-15-2h-1c-3.9 0-7-3.1-7-7v-4h2v4c0 2.8 2.2 5 5 5h1zm-1-18v2h3c1.1 0 2-.9 2-2V4c0-1.1-.9-2-2-2H4c-1.1 0-2 .9-2 2v6c0 1.1.9 2 2 2h2.4l1.7 3l1.7-1l-2.3-4H4V4h10v6z"
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

export default Orders;

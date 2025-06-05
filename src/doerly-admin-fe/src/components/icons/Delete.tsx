import React from 'react';
import SvgIcon from '../SvgIcon';
import useTheme from 'hooks/useTheme';

/* eslint-disable max-len */
const Delete = ({
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
      viewBox="0 0 24 24"
    >
      <g transform="translate(3.0, 1.0)">
        <path
          fill={actualColor}
          d="M7,16 C7.55228475,16 8,15.5522847 8,15 L8,9 C8,8.44771525 7.55228475,8 7,8 C6.44771525,8 6,8.44771525 6,9 L6,15 C6,15.5522847 6.44771525,16 7,16 Z M17,4 L13,4 L13,3 C13,1.34314575 11.6568542,-4.4408921e-16 10,-4.4408921e-16 L8,-4.4408921e-16 C6.34314575,-4.4408921e-16 5,1.34314575 5,3 L5,4 L1,4 C0.44771525,4 0,4.44771525 0,5 C0,5.55228475 0.44771525,6 1,6 L2,6 L2,17 C2,18.6568542 3.34314575,20 5,20 L13,20 C14.6568542,20 16,18.6568542 16,17 L16,6 L17,6 C17.5522847,6 18,5.55228475 18,5 C18,4.44771525 17.5522847,4 17,4 Z M7,3 C7,2.44771525 7.44771525,2 8,2 L10,2 C10.5522847,2 11,2.44771525 11,3 L11,4 L7,4 L7,3 Z M14,17 C14,17.5522847 13.5522847,18 13,18 L5,18 C4.44771525,18 4,17.5522847 4,17 L4,6 L14,6 L14,17 Z M11,16 C11.5522847,16 12,15.5522847 12,15 L12,9 C12,8.44771525 11.5522847,8 11,8 C10.4477153,8 10,8.44771525 10,9 L10,15 C10,15.5522847 10.4477153,16 11,16 Z"
        />
      </g>
    </SvgIcon>
  );
};

interface IProps {
  color?: 'header' | 'page' | string,
  size?: number,
}

export default Delete;

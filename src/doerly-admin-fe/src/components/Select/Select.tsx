import React, { useEffect, useMemo, useRef, useState } from 'react';
import { makeStyles } from 'tss-react/mui';
import SelectMui from '@mui/material/Select';
import FormControlMui from '@mui/material/FormControl';
import InputLabelMui from '@mui/material/InputLabel';

import Checkbox from 'components/Checkbox';
import Highlight from 'components/HighlightText';
import IconSearch from 'components/icons/Search';
import MenuItem from 'components/MenuItem';
import Popover from 'components/Popover';
import Show from 'components/Show';
import TextField from 'components/TextField';
import useTheme from 'hooks/useTheme';

const getClasses = makeStyles<any>()((_, theme: any) => ({
  customPopperContent: {
    display: 'flex',
    flexDirection: 'column',
  },
  customPopperItems: {
    maxHeight: '300px',
    overflowY: 'auto',
  },
  searchContainer: {
    padding: `${theme.spacing(1)}px`,
  },
}));

const Select = ({
  children: inputChildren,
  contentMinWidth = 100,
  disabled = false,
  disableSearch = true,
  disableUnderline = false,
  fullWidth = false,
  label,
  multiple = false,
  onChange,
  onClose,
  onOpen,
  renderValue,
  searchLimit = 10,
  selectedFirst = true,
  size = 'medium',
  value,
  variant = 'standard',
}: IProps) => {
  const { theme } = useTheme();
  const { classes } = getClasses(theme);
  const selectRef = useRef<HTMLDivElement>(null);

  const [state, setState] = useState({
    customPopperAnchorEl: null,
    isFocused: false,
    searchText: '',
    selectedItems: multiple ? value : [],
  });

  const children = makeArray(inputChildren).flat() as any;

  const menuItemsChildren = children
    .filter((child: any) => child.type?.doerlyAdminName === 'MenuItem');

  const restChildren = children
    .filter((child: any) => !child.type?.doerlyAdminName);

  const needSearch = !disableSearch && menuItemsChildren.length >= searchLimit;

  const handleSelectItem = (event: any, menuItemValue: any) => {
    if (!multiple) {
      const eventCopy = {
        ...event,
        target: {
          ...event.target,
          value: menuItemValue,
        },
      };
      onChange(eventCopy);
      if (onClose) {
        onClose(event);
      }
      setState(prev => ({
        ...prev,
        customPopperAnchorEl: null,
        isFocused: false,
      }));
    } else {
      setState(prev => ({
        ...prev,
        selectedItems: prev.selectedItems.includes(menuItemValue)
          ? prev.selectedItems
            .filter((item: any) => item !== menuItemValue)
          : [...prev.selectedItems, menuItemValue],
      }));
    }
  };

  const filteredMenuItems = useMemo(
    () => {
      return state.searchText
        ? menuItemsChildren
          .filter((child: any) => child.props?.searchValues
            ?.some((text: any) => text
              .toString()
              .toUpperCase()
              .includes(state.searchText.toUpperCase())))
        : menuItemsChildren;
    },
    [state.searchText, menuItemsChildren]
  );

  const groupedFilteredMenuItems = useMemo(
    () => {
      if (!selectedFirst || !multiple || !value) {
        return filteredMenuItems;
      }
      const selectedMenuItems = filteredMenuItems
        .filter((item: any) => value.includes(item.props.value));
      const unselectedMenuItems = filteredMenuItems
        .filter((item: any) => !value.includes(item.props.value));
      return [...selectedMenuItems, ...unselectedMenuItems];
    },
    [filteredMenuItems, value]
  );

  const formattedMenuItems = useMemo(
    () => {
      return groupedFilteredMenuItems.map((menuItem: any) => (
        <MenuItem
          {...menuItem.props}
          selected={!multiple && menuItem.props.value === value}
          onClick={(event) => handleSelectItem(event, menuItem.props.value)}
        >
          <Show condition={multiple}>
            <Checkbox
              checked={state.selectedItems.includes(menuItem.props.value)}
              disableHoverSpace
            />
          </Show>
          {menuItem.props.children}
        </MenuItem>
      ));
    },
    [groupedFilteredMenuItems, state.selectedItems, multiple]
  );

  const menuItemsToDisplay = useMemo(
    () => {
      if (!state.searchText) {
        return formattedMenuItems;
      }
      return formattedMenuItems.map((child: any) => (
        <Highlight search={state.searchText}>
          {child}
        </Highlight>
      ));
    },
    [formattedMenuItems]
  );

  const defaultRenderValue = (value: any) => {
    return multiple
      ? menuItemsChildren
        .filter((child: any) => value.includes(child.props.value))
        .map((child: any, index: number) => (
          <>
            {index === 0 ? '' : ', '}
            {child.props?.children}
          </>
        ))
      : menuItemsChildren
        .find((child: any) => child.props.value === value)
        ?.props?.children;
  };

  const handleCloseCustomPopper = (event: any) => {
    if (multiple) {
      const eventCopy = {
        ...event,
        target: {
          ...event.target,
          value: state.selectedItems,
        },
      };
      onChange(eventCopy);
    }
    if (onClose) {
      onClose(event);
    }
    setState(prev => ({
      ...prev,
      customPopperAnchorEl: null,
      isFocused: false,
    }));
  };

  const handleOpen = (event: any) => {
    setState(prev => ({
      ...prev,
      customPopperAnchorEl: event.currentTarget,
      isFocused: true,
    }));
    if (onOpen) {
      onOpen(event);
    }
  };

  const handleSearch = (event: any) => {
    setState(prev => ({
      ...prev,
      searchText: event.target.value,
    }));
  };

  useEffect(
    () => {
      if (multiple) {
        setState(prev => ({
          ...prev,
          selectedItems: value,
        }));
      }
    },
    [value]
  );

  return (
    <>
      <FormControlMui
        focused={state.isFocused}
        fullWidth={fullWidth}
        size={size}
        sx={{
          '& .MuiInput-root': {
            '&:after': {
              borderBottomColor: theme.select.color.focus,
            },
            '&:before': {
              borderBottomColor: theme.select.color.border,
            },
            '&:hover:not(.Mui-disabled):before': {
              borderBottomColor: theme.select.color.focus,
            },
            color: theme.select.color.text,
          },
          '& .MuiInputLabel-root': {
            '&.Mui-focused': {
              color: theme.select.color.focus,
            },
            color: theme.select.color.label,
          },
          '& .MuiOutlinedInput-root': {
            '& fieldset': {
              borderColor: theme.select.color.border,
            },
            '&.Mui-focused fieldset': {
              borderColor: theme.select.color.focus,
            },
            '&:hover fieldset': {
              borderColor: theme.select.color.focus,
            },
            color: theme.select.color.text,
          },
          '& .MuiSelect-icon': {
            color: theme.select.color.text,
          },
        }}
      >
        {label && (
          <InputLabelMui>
            {label}
          </InputLabelMui>
        )}
        <SelectMui
          disabled={disabled}
          disableUnderline={disableUnderline}
          inputProps={{
            sx: variant === 'standard' && {
              paddingLeft: `${theme.spacing(1)}px`,
            },
          }}
          label={label}
          multiple={multiple}
          onChange={onChange}
          onOpen={handleOpen}
          open={false}
          ref={selectRef}
          renderValue={renderValue || defaultRenderValue}
          sx={{
            '.MuiSelect-select': {
              alignItems: 'center',
              display: 'flex',
            },
          }}
          value={value}
          variant={variant}
        />
      </FormControlMui>
      <Popover
        anchorEl={state.customPopperAnchorEl}
        onClose={handleCloseCustomPopper}
        open={!!state.customPopperAnchorEl}
      >
        <div
          className={classes.customPopperContent}
          style={{
            width: Math.max(
              selectRef.current?.getBoundingClientRect().width || 0,
              contentMinWidth
            ),
          }}
        >
          <Show condition={needSearch}>
            <div className={classes.searchContainer}>
              <TextField
                AdornmentEnd={(
                  <IconSearch size={24} />
                )}
                autoFocus
                fullWidth
                onChange={handleSearch}
                size="small"
                value={state.searchText}
              />
            </div>
          </Show>
          <div className={classes.customPopperItems}>
            {menuItemsToDisplay}
            {restChildren}
          </div>
        </div>
      </Popover>
    </>
  );
};

const makeArray = (input: any) => {
  if (!input) {
    return [];
  }
  return Array.isArray(input) ? input : [input];
};

interface IProps {
  children: React.ReactNode,
  contentMinWidth?: number,
  disabled?: boolean,
  disableSearch?: boolean,
  disableUnderline?: boolean,
  fullWidth?: boolean,
  label?: string,
  multiple?: boolean,
  onChange: (event: any) => void,
  onClose?: (event: any) => void,
  onOpen?: (event: any) => void,
  renderValue?: (value: any) => React.ReactNode,
  searchLimit?: number,
  selectedFirst?: boolean,
  size?: 'medium' | 'small',
  value: any,
  variant?: 'standard' | 'outlined',
}

export default Select;

import React, { useState } from 'react';
import { makeStyles } from 'tss-react/mui';
import IconButton from 'components/IconButton';
import Typography from 'components/Typography';
import { IGetCategoryResponse } from 'api/catalog/model';
import IconAdd from 'components/icons/Plus';
import IconEdit from 'components/icons/Edit';
import IconDelete from 'components/icons/Delete';
import IconVisibility from 'components/icons/Visibility';
import IconVisibilityOff from 'components/icons/VisibilityOff';
import IconChevronDown from 'components/icons/ChevronDown';
import IconChevronRight from 'components/icons/ChevronRight';
import useTheme from 'hooks/useTheme';
import useIsMobile from 'hooks/useIsMobile';
import Collapse from 'components/Collapse';
import Show from 'components/Show';
import { useNavigate } from 'react-router-dom';
import * as pages from 'constants/pages';
import pagesURLs from 'constants/pagesURLs';

const ICON_SIZE = 20;

const getClasses = makeStyles<any>()((_, theme: any) => ({
  childrenContainer: {
    borderLeft: `1px solid ${theme.colors.cobalt}`,
    marginLeft: `${theme.spacing(2)}px`,
  },
  deletedNode: {
    color: theme.colors.redDark,
    opacity: 0.9,
    textDecoration: 'line-through',
  },
  disabledAndDeletedNode: {
    color: theme.colors.redDark,
    opacity: 0.5,
    textDecoration: 'line-through',
  },
  disabledNode: {
    opacity: 0.5,
  },
  nodeActions: {
    display: 'flex',
    gap: `${theme.spacing(0.5)}px`,
    transition: 'opacity 0.2s ease-in-out',
  },
  nodeActionsMobile: {
    display: 'grid',
    gap: `${theme.spacing(0.5)}px`,
    gridTemplateColumns: '1fr 1fr',
    gridTemplateRows: '1fr 1fr',
    transition: 'opacity 0.2s ease-in-out',
  },
  nodeContainer: {
    display: 'flex',
    flexDirection: 'column',
  },
  nodeContent: {
    alignItems: 'center',
    borderRadius: '4px',
    cursor: 'pointer',
    display: 'flex',
    flex: 1,
    gap: `${theme.spacing(1)}px`,
    padding: `${theme.spacing(0.5)}px ${theme.spacing(1)}px`,
  },
  nodeContentHovered: {
    backgroundColor: theme.colors.greyDark,
  },
  nodeContentMobile: {
    alignItems: 'flex-start',
    borderRadius: '4px',
    cursor: 'pointer',
    display: 'flex',
    flex: 1,
    flexDirection: 'column',
    gap: `${theme.spacing(1)}px`,
    padding: `0 ${theme.spacing(1)}px`,
  },
  nodeHeader: {
    alignItems: 'center',
    borderRadius: '4px',
    display: 'flex',
    padding: `${theme.spacing(1)}px`,
  },
  nodeTitle: {
    flex: 1,
  },
  nodeTitleMobile: {
    marginBottom: `${theme.spacing(1)}px`,
    width: '100%',
  },
}));

interface TreeNodeProps {
  category: IGetCategoryResponse;
  level?: number;
  onAdd?: (parentId: number) => void;
  onEdit?: (category: IGetCategoryResponse) => void;
  onDelete?: (categoryId: number) => void;
  onToggleEnabled?: (categoryId: number, isEnabled: boolean) => void;
}

const TreeNode: React.FC<TreeNodeProps> = ({
  category,
  level = 0,
  onAdd,
  onEdit,
  onDelete,
  onToggleEnabled,
}) => {
  const { theme } = useTheme();
  const { classes } = getClasses(theme);
  const isMobile = useIsMobile();
  const navigate = useNavigate();
  const [expanded, setExpanded] = useState(true);
  const [isHovered, setIsHovered] = useState(false);
  const hasChildren = category.children && category.children.length > 0;

  const handleToggleExpanded = (e: React.MouseEvent) => {
    e.stopPropagation();
    if (hasChildren) {
      setExpanded(!expanded);
    }
  };

  const handleNodeClick = () => {
    navigate(pagesURLs[pages.catalogFilters]
      .replace(':categoryId', category.id.toString()));
  };

  const getNodeClassName = () => {
    let className = classes.nodeHeader;
    if (category.isDeleted && !category.isEnabled)
      className += ` ${classes.disabledAndDeletedNode}`;
    else if (category.isDeleted)
      className += ` ${classes.deletedNode}`;
    else if (!category.isEnabled)
      className += ` ${classes.disabledNode}`;
    return className;
  };

  const getNodeContentClassName = () => {
    let className = isMobile ? classes.nodeContentMobile : classes.nodeContent;
    if (isHovered) className += ` ${classes.nodeContentHovered}`;
    return className;
  };

  const getNodeTitleClassName = () => {
    return isMobile ? classes.nodeTitleMobile : classes.nodeTitle;
  };

  return (
    <div className={classes.nodeContainer}>
      <div className={getNodeClassName()}>
        <IconButton
          compact
          onClick={handleToggleExpanded}
          visible={hasChildren}
        >
          {expanded ? <IconChevronDown /> : <IconChevronRight />}
        </IconButton>
        <div 
          className={getNodeContentClassName()}
          onClick={handleNodeClick}
          onMouseEnter={() => setIsHovered(true)}
          onMouseLeave={() => setIsHovered(false)}
        >
          <div className={getNodeTitleClassName()}>
            <Typography>
              {category.name}
            </Typography>
            {category.description && (
              <Typography color="secondary">
                {category.description}
              </Typography>
            )}
          </div>

          <Show condition={!category.isDeleted}>
            <div
              className={classes.nodeActions}
              style={{ opacity: isHovered ? 1 : 0 }}
            >
              <IconButton
                onClick={(e) => {
                  e.stopPropagation();
                  onAdd?.(category.id);
                }}
              >
                <IconAdd size={ICON_SIZE} />
              </IconButton>

              <IconButton
                onClick={(e) => {
                  e.stopPropagation();
                  onEdit?.(category);
                }}
              >
                <IconEdit size={ICON_SIZE} />
              </IconButton>

              <IconButton
                onClick={(e) => {
                  e.stopPropagation();
                  onToggleEnabled?.(category.id, !category.isEnabled);
                }}
              >
                {category.isEnabled
                  ? <IconVisibility size={ICON_SIZE}/> :
                  <IconVisibilityOff size={ICON_SIZE} />}
              </IconButton>

              <IconButton
                onClick={(e) => {
                  e.stopPropagation();
                  onDelete?.(category.id);
                }}
              >
                <IconDelete size={ICON_SIZE} />
              </IconButton>
            </div>
          </Show>
        </div>
      </div>

      {hasChildren && (
        <Collapse in={expanded}>
          <div className={classes.childrenContainer}>
            {category.children.map((child) => (
              <TreeNode
                key={child.id}
                category={child}
                level={level + 1}
                onAdd={onAdd}
                onEdit={onEdit}
                onDelete={onDelete}
                onToggleEnabled={onToggleEnabled}
              />
            ))}
          </div>
        </Collapse>
      )}
    </div>
  );
};

export default TreeNode;

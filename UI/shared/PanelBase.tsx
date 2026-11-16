import { Icon, Portal, Scrollable } from "cs2/ui";
import {
  CSSProperties,
  FC,
  ReactElement,
  ReactNode,
  useCallback,
  useEffect,
  useMemo,
  useState,
} from "react";
import { closeIcon } from "shared/icons";
import {
  closeButtonClass,
  closeButtonImageClass,
  getFontScale,
  getRem,
  styleMainScreen,
  stylePanel,
  styleSIPTheme,
  wrapperClass,
} from "shared/style";
import { DragHandle } from "shared/vanilla";
import {
  Tab,
  TabBar,
  TabNav,
} from "shared/vanilla-modules/game-ui/common/tabs/tabs";
import panelStyle from "styles/panel.module.scss";

export interface PanelTab {
  id: string;
  label: ReactNode;
  content: ReactElement;
  footer?: ReactElement;
  disabled?: boolean;
}

interface PanelBaseProps {
  id: string;
  title: string;
  visible: boolean;
  content?: ReactElement;
  footer?: ReactElement;
  tabs?: PanelTab[];
  selectedTab?: string;
  defaultTab?: string;
  onTabChange?: (id: string) => void;
  tabBarClassName?: string;
  height?: number;
  width?: number;
  icon?: string;
  left?: number;
  posX?: number;
  posY?: number;
  sipAligned?: boolean;
  scrollable?: boolean;
  onClose?: () => void;
}

export const PanelBase: FC<PanelBaseProps> = (props: PanelBaseProps) => {
  const [isDragging, setIsDragging] = useState(false);
  const [position, setPosition] = useState<{ x: number; y: number } | null>(
    null,
  );
  const [panelLeft, setPanelLeft] = useState(0);
  const [panelTop, setPanelTop] = useState(0);

  const handleDrag = useCallback(
    (clientX: number, clientY: number, offsetX: number, offsetY: number) => {
      setIsDragging(true);

      setPosition({
        x: clientX - offsetX,
        y: clientY - offsetY,
      });
    },
    [],
  );

  const rem = getRem();
  const fontScale = getFontScale();

  useEffect(() => {
    if (!props.visible) return;

    setPosition({
      x: props.posX ?? panelLeft,
      y: props.posY ?? panelTop,
    });
  }, [props.visible, panelLeft, panelTop, props.posX, props.posY]);

  const wrapperStyle = useMemo<CSSProperties | undefined>(() => {
    if (!position) return undefined;

    return {
      position: "absolute",
      left: `${position.x}px`,
      top: `${position.y}px`,
      ...(props.width !== undefined && {
        width: `${props.width}rem`,
        maxWidth: `${props.width}rem`,
      }),
      ...(props.height !== undefined && {
        height: `${props.height}rem`,
        maxHeight: `${props.height}rem`,
      }),
    };
  }, [position, props.width, props.height]);

  const updatePanelPosition = useCallback(() => {
    const mainScreen = document.querySelector<HTMLElement>(
      `.${styleMainScreen.infoLayout}`,
    );
    if (!mainScreen) return null;

    setPanelTop(mainScreen.offsetTop);

    if (!props.sipAligned) {
      const scale = rem * (0.33333 + fontScale / 1.5);
      const panelWidth = ((props.width ?? 400) + 40) * scale;
      const newPanelLeft =
        mainScreen.offsetWidth + mainScreen.offsetLeft - panelWidth;
      setPanelLeft(newPanelLeft);
      return;
    }

    const sipElement = document.querySelector<HTMLElement>(
      `.${styleMainScreen.selectedInfoPanel}`,
    );
    if (!sipElement) return null;

    const newPanelLeft = mainScreen.offsetLeft * 1.5 + sipElement.offsetWidth;

    setPanelLeft(newPanelLeft);
  }, [props.sipAligned, props.width, rem, fontScale]);

  useEffect(() => {
    updatePanelPosition();

    const observer = new ResizeObserver(() => {
      updatePanelPosition();
    });

    const mainScreen = document.querySelector<HTMLElement>(
      `.${styleMainScreen.infoLayout}`,
    );
    if (!mainScreen) return;

    observer.observe(mainScreen);

    return () => observer.disconnect();
  }, [updatePanelPosition]);

  const {
    tabs,
    selectedTab,
    defaultTab,
    onTabChange,
    scrollable = true,
  } = props;

  const [internalTab, setInternalTab] = useState<string | undefined>(
    defaultTab ?? tabs?.[0]?.id,
  );
  const currentTabId = selectedTab ?? internalTab;

  const handleTabSelect = useCallback(
    (id: string) => {
      setInternalTab(id);
      onTabChange?.(id);
    },
    [onTabChange],
  );

  const tabIds = useMemo(() => tabs?.map(t => t.id) ?? [], [tabs]);
  const hasTabs = tabIds.length > 0;
  const activeTab = tabs?.find(t => t.id === currentTabId) ?? tabs?.[0];

  const body = hasTabs ? activeTab?.content : props.content;
  const footer = (hasTabs ? activeTab?.footer : undefined) ?? props.footer;

  if (!props.visible) return null;

  return (
    <>
      <Portal>
        <div
          id={`starq-panel-${props.id}`}
          className={`${wrapperClass} draggable-panel ${panelStyle.Panel} ${isDragging ? panelStyle.dragging : ""} ${panelStyle.AnimateIn}`}
          style={wrapperStyle}
        >
          <DragHandle
            onDrag={handleDrag}
            onDragStart={() => setIsDragging(true)}
            onDragEnd={() => setIsDragging(false)}
          >
            <div
              id={`starq-panel-header-${props.id}`}
              className={styleSIPTheme.header}
            >
              <div className={stylePanel.titleBar}>
                {props.icon && (
                  <Icon className={stylePanel.icon} src={props.icon} />
                )}
                <div className={styleSIPTheme.title}>{props.title}</div>
                <button className={closeButtonClass} onClick={props.onClose}>
                  <div
                    className={closeButtonImageClass}
                    style={{
                      maskImage: `url(${closeIcon})`,
                    }}
                  ></div>
                </button>
              </div>
            </div>
          </DragHandle>
          <div
            id={`starq-panel-content-${props.id}`}
            className={`${styleSIPTheme.content} ${panelStyle.Container} ${panelStyle.View}`}
          >
            {hasTabs && (
              <TabNav
                tabs={tabIds}
                selectedTab={activeTab?.id}
                onSelect={handleTabSelect}
              >
                <TabBar className={props.tabBarClassName}>
                  {tabs!.map(t => (
                    <Tab
                      key={t.id}
                      id={t.id}
                      selectedId={activeTab?.id}
                      disabled={t.disabled}
                      onSelect={handleTabSelect}
                    >
                      {t.label}
                    </Tab>
                  ))}
                </TabBar>
              </TabNav>
            )}
            {scrollable ? (
              <Scrollable className={panelStyle.Body}>
                <div>{body}</div>
              </Scrollable>
            ) : (
              <div className={panelStyle.Body}>{body}</div>
            )}
            {footer && <div className={panelStyle.Footer}>{footer}</div>}
          </div>
        </div>
      </Portal>
    </>
  );
};

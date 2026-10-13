import { Icon, Portal, Scrollable } from "cs2/ui";
import {
  CSSProperties,
  FC,
  ReactElement,
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
import panelStyle from "styles/panel.module.scss";

interface PanelBaseProps {
  id: string;
  title: string;
  visible: boolean;
  content: ReactElement;
  footer?: ReactElement;
  height?: number;
  icon?: string;
  left?: number;
  posX?: number;
  posY?: number;
  sipAligned?: boolean;
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
    };
  }, [position]);

  const rem = getRem();
  const fontScale = getFontScale();

  const updatePanelPosition = useCallback(() => {
    const mainScreen = document.querySelector<HTMLElement>(
      `.${styleMainScreen.infoLayout}`,
    );
    if (!mainScreen) return null;

    setPanelTop(mainScreen.offsetTop);

    if (!props.sipAligned) {
      const sipWidth = (400 + 40) * rem * (0.33333 + fontScale / 1.5);
      const newPanelLeft =
        mainScreen.offsetWidth + mainScreen.offsetLeft - sipWidth;
      setPanelLeft(newPanelLeft);
      return;
    }

    const sipElement = document.querySelector<HTMLElement>(
      `.${styleMainScreen.selectedInfoPanel}`,
    );
    if (!sipElement) return null;

    const newPanelLeft = mainScreen.offsetLeft * 1.5 + sipElement.offsetWidth;

    setPanelLeft(newPanelLeft);
  }, [props.sipAligned, rem, fontScale]);

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
            <Scrollable>
              <div>{props.content}</div>
            </Scrollable>
            <div>{props.footer}</div>
          </div>
        </div>
      </Portal>
    </>
  );
};

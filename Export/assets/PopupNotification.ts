import UISprite from './UI/UISprite';
import UIPopup from './UI/UIPopup';
import UILabel from './UI/UILabel';
import UIButton from './UI/UIButton';
import * as SC from './SupportClasses';

export default class PopupNotification extends UIPopup {
	public ImageBackground: UISprite = new UISprite("images/PopupContentBackground.png", 0, 0);
	public ButtonClose: SC.UIButtonTextLabelUILabel;
	public TextNotification: UILabel = new UILabel("TextNotification", 40, 0xFF0075, 270, 115);

	constructor(x: number, y: number) {
		super(x, y);

		const ImageBackground14 = new UISprite("images/PopupContentBackground.png", 0, 0, );

		this.ImageBackground = ImageBackground14;
		this.addChild(this.ImageBackground);

		const TextLabel16 = new UILabel("Button", 24, 0x0639FF, 75, 30, );
		const ButtonClose15 = new SC.UIButtonTextLabelUILabel("images/PopupButton.png", 375, 15, TextLabel16);

		this.ButtonClose = ButtonClose15;
		this.addChild(this.ButtonClose);

		const TextNotification17 = new UILabel("TextNotification", 40, 0xFF0075, 270, 115, );

		this.TextNotification = TextNotification17;
		this.addChild(this.TextNotification);
	};
}

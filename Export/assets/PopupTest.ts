import UISprite from './UI/UISprite';
import UIPopup from './UI/UIPopup';
import UILabel from './UI/UILabel';
import UIButton from './UI/UIButton';

export default class PopupTest extends UIPopup {
	public one = new class extends UISprite {
		public Image = new class extends UISprite {
			public TextLabel = new UILabel("Popup Label", 60, 0x00FFF1, 222, 46.5);
			constructor() {
				super("images/PopupHeaderFooterBackground.png", 63, 17);
				this.addChild(this.TextLabel);
			}
		}
		public panel = new class extends UISprite {
			public buttons = new class extends UISprite {
				public accept = new class extends UIButton {
					public TextLabel = new UILabel("Accept", 24, 0x0639FF, 75, 30);
					constructor() {
						super("images/PopupButton.png", 147, 16.5);
						this.addChild(this.TextLabel);
					}
				}
				constructor() {
					super("images/PopupHeaderFooterBackground.png", 48, 28.5);
					this.addChild(this.accept);
				}
			}
			constructor() {
				super("images/PopupContentBackground.png", 15, 127.5);
				this.addChild(this.buttons);
			}
		}
		constructor() {
			super("images/PopupBackground.png", -15, -127.5);
			this.addChild(this.Image);
			this.addChild(this.panel);
		}
	}
	public two = new class extends UISprite {
		public Label1 = new UILabel("Label1", 36, 0xFF8900, 112, 51.5);
		public Label2 = new UILabel("Label2", 36, 0xFF008B, 285, 51.5);
		public Label3 = new UILabel("Label3", 36, 0x29FF00, 466, 51.5);
		public panel = new class extends UISprite {
			public buttons = new class extends UISprite {
				public reject = new class extends UIButton {
					public TextLabel = new UILabel("Reject", 24, 0x0639FF, 75, 30);
					constructor() {
						super("images/PopupButton.png", 38, 16.5);
						this.addChild(this.TextLabel);
					}
				}
				public cancel = new class extends UIButton {
					public TextLabel = new UILabel("Cancel", 24, 0x0639FF, 75, 30);
					constructor() {
						super("images/PopupButton.png", 254, 16.5);
						this.addChild(this.TextLabel);
					}
				}
				constructor() {
					super("images/PopupHeaderFooterBackground.png", 48, 28.5);
					this.addChild(this.reject);
					this.addChild(this.cancel);
				}
			}
			constructor() {
				super("images/PopupContentBackground.png", 15, 127.5);
				this.addChild(this.buttons);
			}
		}
		constructor() {
			super("images/PopupBackground.png", -15, 277.5);
			this.addChild(this.Label1);
			this.addChild(this.Label2);
			this.addChild(this.Label3);
			this.addChild(this.panel);
		}
	}
	constructor() {
		super(429.9999, 211.3684);
		this.addChild(this.one);
		this.addChild(this.two);
	}
}

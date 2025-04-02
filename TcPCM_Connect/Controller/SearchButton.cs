using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace TcPCM_Connect.Controller
{
    public partial class SearchButton : UserControl
    {
        private TextBox searchTextBox;
        public Button detailSearchButton;

        public SearchButton()
        {
            InitializeComponent();
            this.Size = new Size(436, 30); // 컴포넌트 기본 크기 설정

            // Panel for Search Box
            Panel searchPanel = new Panel();
            searchPanel.BorderStyle = BorderStyle.FixedSingle;
            searchPanel.Size = new Size(300, 30); // 검색창 크기
            searchPanel.Location = new Point(10, 0);
            searchPanel.BackColor = Color.WhiteSmoke; // 텍스트 박스 배경색 초기화
            searchPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
| System.Windows.Forms.AnchorStyles.Left)
| System.Windows.Forms.AnchorStyles.Right)));

            // TextBox inside Search Box
            searchTextBox = new TextBox();
            searchTextBox.BorderStyle = BorderStyle.None;
            searchTextBox.Location = new Point(10, 5);
            searchTextBox.Width = 220; // 텍스트 박스 크기 조정
            searchTextBox.BackColor = Color.WhiteSmoke; // 기본 텍스트 박스 배경색
            searchTextBox.ForeColor = this.ForeColor; // 기본 글자색 동기화
            searchTextBox.Font = new Font("Arial", 10); // 텍스트 폰트
            searchTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
| System.Windows.Forms.AnchorStyles.Left)
| System.Windows.Forms.AnchorStyles.Right)));

            // 엔터 키 이벤트
            searchTextBox.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true; // 엔터 키의 기본 동작 방지
                    OnSearchButtonClick(EventArgs.Empty); // SearchButton 클릭 이벤트 호출
                }
            };

            // Search Button (돋보기 아이콘)
            Button searchButton = new Button();
            searchButton.FlatStyle = FlatStyle.Flat;
            searchButton.Image = global::TcPCM_Connect.Properties.Resources._3844432_magnifier_search_zoom_icon; // 돋보기 아이콘
            searchButton.ImageAlign = ContentAlignment.MiddleCenter;
            searchButton.Size = new Size(24, 24);
            searchButton.Location = new Point(270, 2); // 버튼 위치 조정 (오른쪽으로 이동)
            searchButton.FlatAppearance.BorderSize = 0;
            searchButton.BackColor = Color.WhiteSmoke;
            searchButton.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | AnchorStyles.Right;
            // 버튼 클릭 이벤트
            searchButton.Click += (s, e) =>
            {
                OnSearchButtonClick(EventArgs.Empty); // 외부로 노출된 이벤트 호출
            };

            // Add TextBox and Search Button to Panel
            searchPanel.Controls.Add(searchTextBox);
            searchPanel.Controls.Add(searchButton);

            // Detail Search Button
            detailSearchButton = new Button();
            detailSearchButton.Text = "상세 검색";
            detailSearchButton.Image = global::TcPCM_Connect.Properties.Resources._3671698_filter_icon; // 필터 아이콘
            detailSearchButton.FlatStyle = FlatStyle.Flat;
            detailSearchButton.Size = new Size(100, 30); // 버튼 크기 조정
            detailSearchButton.Location = new Point(320, 0); // 검색창 오른쪽에 배치
            detailSearchButton.FlatAppearance.BorderSize = 0;
            detailSearchButton.Font = new Font("Arial", 12);
            detailSearchButton.TextAlign = ContentAlignment.MiddleLeft; // 텍스트 왼쪽 정렬
            detailSearchButton.ImageAlign = ContentAlignment.MiddleRight; // 아이콘 오른쪽 정렬
            detailSearchButton.BackColor = Color.White; // 기본 상세 검색 버튼 배경색
            detailSearchButton.ForeColor = this.ForeColor; // 기본 글자색 동기화
            detailSearchButton.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | AnchorStyles.Right;

            // 상세 검색 버튼 클릭 이벤트
            detailSearchButton.Click += (s, e) =>
            {
                OnDetailSearchButtonClick(EventArgs.Empty); // 외부로 노출된 이벤트 호출
            };

            // Add controls to UserControl
            this.Controls.Add(searchPanel);
            this.Controls.Add(detailSearchButton);

            // 속성 초기화
            TextBoxBackColor = Color.WhiteSmoke;
            DetailSearchButtonBackColor = Color.White;
            PanelBackColor = Color.Transparent;
            this.BackColor = Color.Transparent;
        }

        // 검색 버튼 클릭 이벤트 외부 노출
        [Category("Action")]
        [Description("검색 버튼이 클릭될 때 발생합니다.")]
        [Browsable(true)]
        public event EventHandler SearchButtonClick;

        // 상세 검색 버튼 클릭 이벤트 외부 노출
        [Category("Action")]
        [Description("상세 검색 버튼이 클릭될 때 발생합니다.")]
        [Browsable(true)]
        public event EventHandler DetailSearchButtonClick;

        // 검색 버튼 클릭 이벤트 호출
        protected virtual void OnSearchButtonClick(EventArgs e)
        {
            SearchButtonClick?.Invoke(this, e);
        }

        // 상세 검색 버튼 클릭 이벤트 호출
        protected virtual void OnDetailSearchButtonClick(EventArgs e)
        {
            DetailSearchButtonClick?.Invoke(this, e);
        }

        // ForeColor 속성 재정의
        [Category("Appearance")]
        [Description("텍스트 박스와 버튼의 텍스트 색상을 설정합니다.")]
        public override Color ForeColor
        {
            get => base.ForeColor;
            set
            {
                base.ForeColor = value;
                if (searchTextBox != null)
                    searchTextBox.ForeColor = value; // 텍스트 박스 색상 변경
                if (detailSearchButton != null)
                    detailSearchButton.ForeColor = value; // 상세 검색 버튼 색상 변경
            }
        }

        // TextBox 배경색 속성
        [Category("Appearance")]
        [Description("텍스트 박스의 배경색을 설정합니다.")]
        public Color TextBoxBackColor
        {
            get => searchTextBox.BackColor;
            set
            {
                if (searchTextBox != null)
                    searchTextBox.BackColor = value;
            }
        }


        [Category("Appearance")]
        [Description("텍스트 박스의 내용을 설정합니다.")]
        public string text
        {
            get => searchTextBox.Text;
            set
            {
                if (searchTextBox != null)
                    searchTextBox.Text = value;
            }
        }
        // DetailSearchButton 배경색 속성
        [Category("Appearance")]
        [Description("상세 검색 버튼의 배경색을 설정합니다.")]
        public Color DetailSearchButtonBackColor
        {
            get => detailSearchButton.BackColor;
            set
            {
                if (detailSearchButton != null)
                    detailSearchButton.BackColor = value;
            }
        }

        // Panel 배경색 속성
        [Category("Appearance")]
        [Description("검색 패널의 배경색을 설정합니다.")]
        public Color PanelBackColor
        {
            get => this.BackColor;
            set
            {
                this.BackColor = value;
            }
        }
    }
}

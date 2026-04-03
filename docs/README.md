# 웹 포트폴리오 (GitHub Pages)

이 폴더는 게임 프로젝트 시연 및 소개를 위한 웹 포트폴리오 페이지입니다.  
GitHub Pages 배포용 정적 파일을 포함하고 있습니다.

## 로컬 미리보기
`index.html`을 브라우저에서 열면 확인할 수 있습니다.

## 배포 (GitHub Pages)
이 저장소 루트가 아니라 `docs/` 폴더를 GitHub Pages 대상으로 사용합니다.

1. GitHub 저장소로 이동합니다.
2. **Settings → Pages** 로 들어갑니다.
3. **Build and deployment**에서  
   - Source: `Deploy from a branch`
   - Branch: `main`
   - Folder: `/docs`
4. 저장 후 잠시 기다리면 Pages 주소가 생성됩니다.

예시:
`https://USERNAME.github.io/GameClient-Portfolio`

## 구조

- `index.html`: 웹 포트폴리오 메인 페이지
- `css/style.css`: 스타일 파일
- `js/main.js`: 인터랙션 스크립트
- `assets/`: 이미지 및 시연용 미디어 파일

## 안내

이 웹 페이지는 프로젝트 결과와 시연 중심으로 구성되어 있으며,  
상세한 코드 구조 및 시스템 설계 설명은 저장소 루트의 코드 포트폴리오 README를 참고하면 됩니다.
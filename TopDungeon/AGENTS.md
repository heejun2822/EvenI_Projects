# AGENTS.md

## 프로젝트 개요
- Unity로 만든 개인 학습용 소규모 게임 프로젝트입니다.
- 현재 목표: 핵심 플레이 루프를 간단하고 읽기 쉽게 구현합니다.
- 과도한 추상화나 새 패키지 추가는 피합니다.

## 기술 정보
- Unity 버전: `6000.3.11f1`
- 언어: C#
- 주요 코드 위치: `Assets/Scripts/`
- 씬 위치: `Assets/Scenes/`
- 프리팹 위치: `Assets/Prefabs/`

## 코드 원칙
- 한 스크립트는 하나의 역할에 집중합니다.
- Inspector에서 조정할 값은 `[SerializeField] private`로 둡니다.
- 매직 넘버는 의미 있는 변수 또는 상수로 바꿉니다.
- 기존 네이밍과 폴더 구조를 우선 따릅니다.
- 필요하지 않은 싱글턴, 이벤트 시스템, 범용 프레임워크는 추가하지 않습니다.

## Unity 파일 주의사항
- `.meta` 파일은 에셋 참조에 필요하므로 삭제하거나 임의로 수정하지 않습니다.
- `Library/`, `Temp/`, `Logs/`, `obj/`는 수정하지 않습니다.
- 씬·프리팹·ProjectSettings 변경은 꼭 필요할 때만 하고, 변경 이유를 보고합니다.
- 새 에셋·패키지 추가 전에는 먼저 확인받습니다.

## 작업 방식
- 변경 전에 관련 스크립트와 호출 위치를 먼저 확인합니다.
- 작은 단위로 수정하고, 수정한 파일과 이유를 요약합니다.
- Unity Editor 실행이 가능하면 Console 오류가 없는지 확인합니다.
- 실행 검증을 하지 못했다면 그 사실과 수동 확인 방법을 알려줍니다.

## graphify

This project has a knowledge graph at graphify-out/ with god nodes, community structure, and cross-file relationships.

When the user types `/graphify`, use the installed graphify skill or instructions before doing anything else.

Rules:
- For codebase questions, first run `graphify query "<question>"` when graphify-out/graph.json exists. Use `graphify path "<A>" "<B>"` for relationships and `graphify explain "<concept>"` for focused concepts. These return a scoped subgraph, usually much smaller than GRAPH_REPORT.md or raw grep output.
- Dirty graphify-out/ files are expected after hooks or incremental updates; dirty graph files are not a reason to skip graphify. Only skip graphify if the task is about stale or incorrect graph output, or the user explicitly says not to use it.
- If graphify-out/wiki/index.md exists, use it for broad navigation instead of raw source browsing.
- Read graphify-out/GRAPH_REPORT.md only for broad architecture review or when query/path/explain do not surface enough context.
- After modifying code, run `graphify update .` to keep the graph current (AST-only, no API cost).

# Tower Defense CG-RV 2026/2

Projeto de um jogo **Tower Defense 2D**, desenvolvido para a disciplina de Computação Gráfica e Realidade Virtual.

## 1. Informações gerais

- **Engine:** Unity 6000.3.15f1 LTS
- **Plataforma:** PC
- **Modelo inicial:** Universal 2D (URP)

## 2. Estruturas defensivas

- [ ] Implementar no mínimo duas estruturas defensivas distintas.
- [x] **Estrutura 1:** característica 1.
- [ ] **Estrutura 2:** característica 2.

## 3. Níveis de dificuldade

- [ ] Implementar no mínimo dois níveis de dificuldade.
- [ ] No nível mais simples, pelo menos 5 inimigos deverão ser eliminados.
- [ ] A quantidade e a variação de inimigos devem depender do nível de dificuldade escolhido.

## 4. Menu principal e opções

O jogo deverá possuir um menu de opções que permita:

- [ ] Escolher o nível de dificuldade.
- [ ] Iniciar o jogo.
- [ ] Visualizar um ranking simples.
- [ ] Consultar informações sobre o jogo.

## 5. Área de jogo e cenário

- [x] Implementar a base ou estrutura a ser defendida.
- [x] Implementar um ou mais pontos de entrada dos inimigos. 
  - **Observação:** um ponto.
- [x] Implementar um caminho ou conjunto de caminhos pelos quais os inimigos deverão se deslocar.
  - **Observação:** um caminho.
- [ ] Implementar posições ou áreas para a colocação das estruturas defensivas.
- [ ] O cenário deve possuir tamanho suficiente para comportar, no mínimo, 30 inimigos enfileirados, considerando os modelos utilizados.

## 6. Inimigos

### 6.1. Comportamento e movimentação

- [x] Deslocar-se até a base ou objetivo final.
- [x] Causar algum tipo de consequência ao atingir o objetivo final.

### 6.2. Resistência e dano

- [x] Possuir algum mecanismo de resistência ou vida.
- [x] Ser capazes de receber dano.
- [x] Ser destruídos quando sua vida chegar a zero.

### 6.3. Variedade

- [ ] É recomendado que os inimigos representem pelo menos dois comportamentos ou características distintas.
- [ ] A quantidade e a variação de inimigos devem depender do nível de dificuldade escolhido.

## 7. Ataques e projéteis

- [x] Criar ou ativar projéteis pelas estruturas defensivas.
- [x] Implementar objetos que se desloquem até os inimigos ou até saírem da área do jogo.
- [x] Detectar colisões entre projéteis e inimigos.
- [x] Causar dano aos inimigos.
- [x] Destruir ou desativar os projéteis após atingirem um inimigo ou saírem da área válida.

## 8. Colisões

Implementar os seguintes tipos de colisão:

- [x] Impacto entre projéteis e inimigos.
- [x] Chegada dos inimigos até a base.
- [x] Destruição ou remoção dos inimigos.

## 9. Condições de vitória e derrota

**Escolher apenas uma das seguintes condições:**

- [x] A vida da base chegar a zero.
- [ ] Uma quantidade X de inimigos chegar à base.
- [ ] O tempo máximo acabar sem a conclusão do objetivo.

Requisito temporal:

- [ ] Implementar um tempo máximo para a partida.

## 10. Entregáveis

- [ ] Disponibilizar um link para baixar o executável do jogo.
- [ ] Disponibilizar um link para um vídeo de demonstração da solução.

## 11. Critérios de avaliação

| Critério | Pontuação |
|---|---:|
| Menu principal, com níveis de dificuldade, ranking e informações sobre o jogo | 0,5 |
| Qualidade da modelagem das estruturas defensivas, dos inimigos e do cenário: quantidade e variedade de objetos | 1,0 |
| Movimentação correta dos inimigos, comportamento das estruturas defensivas e funcionamento dos projéteis | 2,5 |
| Nível de interatividade: facilidade de controle, velocidade de execução, gerenciamento de alvos e detecção de colisões | 2,5 |
| Utilização de efeitos visuais e recursos sonoros | 1,0 |
| Demonstração em vídeo | 0,5 |
| **Individual: contribuições no GitHub e domínio do projeto durante a apresentação** | **2,0** |
| **Total** | **10,0** |

## 12. Pontos extras

- [ ] Implementar suporte à Realidade Virtual (VR) ou Realidade Aumentada (AR).

**Pontuação extra:** até 1,0 ponto.

Os pontos extras podem ser cumulativos com as demais avaliações da disciplina, estando sujeitos ao nível de domínio do projeto.
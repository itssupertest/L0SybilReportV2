import matplotlib.pyplot as plt
import networkx as nx

#Define the input data for CEX nodes
data = {
    "CEX 0x1775": [("0x48d7", 3),("0x95cb", 4),("0x5d10", 1)],
    "CEX 0x3229": [("0x48d7", 34),("0x5917", 1),("0x45ab", 6),("0x95cb", 1),("0x5d10", 1)],
    "CEX 0x46ee": [("0xaf8f", 1),("0x48d7", 7),("0x3578", 1),("0x5917", 3),("0x95cb", 2),("0x45ab", 1)],
    "CEX 0x4b50": [("0xcd07", 1),("0x04be", 1),("0x3f5a", 1),("0xbc55", 3),("0x3578", 1),("0x70c4", 4),("0x48d7", 28),("0x95cb", 2),("0x5917", 3),("0xf345", 2),("0xe41d", 1),("0x5d10", 1),("0x45ab", 8)],
    "CEX 0x4f98": [("0x48d7", 4),("0x6223", 1),("0x95cb", 1),("0x45ab", 1)],
    "CEX 0x5193": [("0xbc55", 1),("0x48d7", 7),("0x95cb", 1)],
    "CEX 0x7201": [("0x48d7", 4),("0x5d10", 2),("0x95cb", 1),("0x45ab", 2)],
    "CEX 0x7a02": [("0x48d7", 15),("0x5917", 3),("0x45ab", 1),("0x95cb", 1),("0xbc55", 1),("0x5d10", 1)],
    "CEX 0xb426": [("0x48d7", 27),("0x5917", 5),("0x45ab", 2),("0xe41d", 2),("0x95cb", 2),("0x5d10", 2),("0xcd07", 1),("0x04be", 1),("0x3f5a", 1),("0xf345", 1),("0x3578", 1),("0xbc55", 1),("0x70c4", 1)],
}

# data = {
# "Distribution address 0x6176": [("0x48d7", 18),("0x95cb", 4),("0x5917", 1),("0xbc55", 1),("0x70c4", 2),("0x5d10", 5),("0xf345", 1),("0x45ab", 4)],
# }

# Create a directed graph
G = nx.DiGraph()

# Add edges to the graph
for node, edges in data.items():
    for edge, weight in edges:
        G.add_edge(node, edge, weight=weight)

# Draw the graph
plt.figure(figsize=(30, 30))  # Increase the figure size for better visibility

# Use circular layout for better spacing
pos = nx.circular_layout(G)

# Define node colors, making CEX nodes a different color
node_colors = []
node_sizes = []
for node in G.nodes():
    if node.startswith("CEX") or node.startswith("Distribution"):
        node_colors.append('red')
        node_sizes.append(5000)
    else:
        node_colors.append('lightblue')
        node_sizes.append(3000)

# Draw nodes and edges
nx.draw(G, pos, with_labels=True, node_size=node_sizes, node_color=node_colors, font_size=10, font_weight='bold', arrows=False)
edge_labels = nx.get_edge_attributes(G, 'weight')

# Draw all edges
nx.draw_networkx_edges(G, pos, edgelist=G.edges(), arrows=False)

# Draw edge labels
# nx.draw_networkx_edge_labels(G, pos, edge_labels=edge_labels)

plt.title('Graph Visualization of CEX Nodes')
plt.show()
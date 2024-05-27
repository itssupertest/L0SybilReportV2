import matplotlib.pyplot as plt
import networkx as nx

# Input data
# Connection "0xaf8f": [("0x48d7", 1)] need to be added manually since its indirect
# Connection "0x48d7": [("0x45ab", 1)], need to be added manually since number of transactions exceed allowed number and DeBank doesnt return it all
data = {
    "0x0062": [("0x48d7", 1),("0x45ab", 1)],
    "0x00bf": [("0x48d7", 1),("0x48d7", 1)],
    "0x04be": [("0x48d7", 1),("0x70c4", 1)],
    "0x0663": [("0x48d7", 1),("0x48d7", 1)],
    "0x0699": [("0x5917", 1),("0x48d7", 1)],
    "0x1134": [("0x7842", 1),("0x7842", 1)],
    "0x13b5": [("0x48d7", 1),("0x48d7", 1)],
    "0x1941": [("0x48d7", 1),("0x5d10", 1)],
    "0x3578": [("0x70c4", 1),("0x95cb", 1)],
    "0x3720": [("0x48d7", 1),("0x45ab", 1)],
    "0x3f5a": [("0xbc55", 1),("0xcd07", 1)],
    "0x45ab": [],
    "0x48d7": [("0x45ab", 1)],
    "0x4b98": [("0xafd1", 1),("0xe59b", 1)],
    "0x51d4": [("0x48d7", 1),("0x45ab", 1)],
    "0x5917": [("0x48d7", 1),("0x48d7", 1)],
    "0x59d6": [("0x0663", 1),("0x45ab", 1)],
    "0x5d10": [("0x70c4", 1),("0x48d7", 1)],
    "0x6223": [("0x1134", 1),("0x1134", 1)],
    "0x6b76": [("0x48d7", 1),("0x70c4", 1)],
    "0x70c4": [("0x48d7", 1),("0x48d7", 1)],
    "0x7487": [("0x48d7", 1),("0x5d10", 1)],
    "0x7842": [("0xa890", 1),("0xa890", 1)],
    "0x8b55": [("0x48d7", 1),("0x95cb", 1)],
    "0x8da6": [("0x48d7", 1),("0x48d7", 1)],
    "0x9074": [("0x4b98", 1),("0xe59b", 1)],
    "0x95cb": [("0x45ab", 1),("0x45ab", 1)],
    "0x9afe": [("0xf788", 1),("0xe59b", 1)],
    "0xa7ce": [("0x48d7", 1),("0xe59b", 1)],
    "0xa890": [("0xe093", 1),("0x5d10", 1)],
    "0xacd4": [("0x48d7", 1),("0x48d7", 1)],
    "0xada1": [("0x48d7", 1),("0x48d7", 1)],
    "0xaf8f": [("0x48d7", 1)],
    "0xafd1": [("0xe59b", 1),("0xe59b", 1)],
    "0xbc55": [("0x70c4", 1),("0x70c4", 1)],
    "0xcd07": [("0x48d7", 1),("0xf345", 1)],
    "0xe093": [("0xfd39", 1),("0x48d7", 1)],
    "0xe41d": [("0x04be", 1)],
    "0xe59b": [("0x0699", 1),("0x48d7", 1)],
    "0xf345": [("0x70c4", 1),("0x3578", 1)],
    "0xf788": [("0x9074", 1),("0xe59b", 1)],
    "0xf8ee": [("0x48d7", 1),("0x59d6", 1)],
    "0xfcf7": [("0x48d7", 1),("0x48d7", 1)],
    "0xfd39": [("0xfe2e", 1),("0xe59b", 1)],
    "0xfe2e": [("0x48d7", 1),("0xe59b", 1)],
}

# Create a directed graph
G = nx.DiGraph()

# Add edges to the graph
for node, edges in data.items():
    for edge, weight in edges:
        G.add_edge(edge, node)

# Separate the special edge for red color
special_edge = ("0x48d7", "0xaf8f")

# Draw the graph
plt.figure(figsize=(20, 20))  # Increase the figure size for better visibility

# Use Circular layout for better spacing
pos = nx.circular_layout(G)

# Define node colors and sizes
node_colors = ['lightblue' for _ in G.nodes()]
node_sizes = [3000 for _ in G.nodes()]

# Draw nodes and edges
nx.draw(G, pos, with_labels=True, node_size=node_sizes, node_color=node_colors, font_size=10, font_weight='bold', arrows=False)

# Draw all edges except the special one
edges = [e for e in G.edges() if e != special_edge]
nx.draw_networkx_edges(G, pos, edgelist=edges, arrows=False)

# Draw the special edge in red
nx.draw_networkx_edges(G, pos, edgelist=[special_edge], edge_color='red', arrows=False, width=2)

# Show the plot
plt.title('Graph Visualization with Circular Layout and Highlighted Edge')
plt.show()